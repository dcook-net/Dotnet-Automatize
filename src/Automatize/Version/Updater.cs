using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Automatize.Extensions;
using Automatize.FileFinders;
using McMaster.Extensions.CommandLineUtils;

namespace Automatize.Version
{
    public class Updater
    {
        private const StringComparison InvariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
        private readonly IFileSystem _fileSystem;
        private readonly IDotNetVersionUpdater _dotNetVersionUpdater;
        private readonly IConsole _console;
        private readonly IProjectFileFinder _projectFileFileFinder;
        private readonly IDockerFileFinder _dockerFileFinder;
        private readonly IEnvironmentFileFinder _environmentFileFinder;

        public Updater(IDotNetVersionUpdater dotNetVersionUpdater,
            IConsole console,
            IProjectFileFinder projectFileFileFinder,
            IDockerFileFinder dockerFileFinder,
            IEnvironmentFileFinder environmentFileFinder, 
            IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _dotNetVersionUpdater = dotNetVersionUpdater;
            _console = console;
            _projectFileFileFinder = projectFileFileFinder;
            _dockerFileFinder = dockerFileFinder;
            _environmentFileFinder = environmentFileFinder;
        }

        public void UpgradeToVersion(string path, bool useLinuxBaseImage)
        {
            //Push these down to the methods below?
            var dockerFiles = _dockerFileFinder.Search(path);
            var projectFiles = _projectFileFileFinder.Search(path);
            var envFiles = _environmentFileFinder.Search(path);

            //Probably ought to pass the target version number in here, 
            //then work out which updater to use, rather than supply it on constructor.

            UpdateProjectFiles(projectFiles, useLinuxBaseImage);
            UpdateDockerFiles(dockerFiles, useLinuxBaseImage);
            UpdateEnvironmentFiles(envFiles, useLinuxBaseImage);
        }

        private void UpdateProjectFiles(IEnumerable<FileInfo> projectFiles, bool useLinuxBaseImage)
        {
            UpdateFiles(projectFiles, UpdateProjectFileContents, "No Project files found!", useLinuxBaseImage);
        }

        private void UpdateDockerFiles(IEnumerable<FileInfo> dockerFiles, bool useLinuxBaseImage)
        {
            UpdateFiles(dockerFiles, UpdateDockerFileContent, "No Docker files found!", useLinuxBaseImage);
        }

        private void UpdateEnvironmentFiles(IEnumerable<FileInfo> envFiles, bool useLinuxBaseImage)
        {
            UpdateFiles(envFiles, UpdateEnvFileContent, "No .env files found!", useLinuxBaseImage);
        }

        private void UpdateFiles(IEnumerable<FileInfo> filesToUpdate, Func<string, bool, string> updateMethod, string noFilesFoundMessage, bool useLinuxBaseImage)
        {
            if (filesToUpdate is null || !filesToUpdate.Any())
            {
                _console.WriteLine(noFilesFoundMessage);
                return;
            }

            foreach (var fileInfo in filesToUpdate)
            {
                _console.WriteLine($"Updating {fileInfo.FullName}");
                try
                {
                    string content;
                    using (var streamReader = _fileSystem.File.OpenText(fileInfo.FullName))
                    {
                        content = streamReader.ReadToEnd();
                    }

                    var updatedContent = updateMethod(content, useLinuxBaseImage);

                    _fileSystem.File.WriteAllText(fileInfo.FullName, updatedContent);
                }
                catch (Exception e)
                {
                    _console.WriteLine($"Updating {fileInfo.FullName} failed: {e.Message}");
                }
            }
        }

        private void UpdateTargetFrameworks(XPathNavigator targetFrameWorksNode, string targetFramework)
        {
            targetFrameWorksNode?.SetValue(targetFrameWorksNode.Value.Replace(_dotNetVersionUpdater.CurrentFramework, targetFramework));
        }

        private void UpdateTargetFramework(XPathNavigator targetFrameWorkNode, string targetFramework)
        {
            if (targetFrameWorkNode != null && !targetFrameWorkNode.Value.Contains("netstandard"))
            {
                targetFrameWorkNode.SetValue(targetFramework);
            }
        }

        private bool PackagesNeedUpdating(XPathNavigator xPathNavigator, string minimumCommonLibVersion)
        {
            var packages = xPathNavigator.Select(@"/Project/ItemGroup/PackageReference[starts-with(@Include, ""MicroMachines.Common"")]");
            var packagesNeedUpdating = false;

            while (packages.MoveNext())
            {
                var packageVersion = packages.Current.GetAttribute("Version", "");

                if (packageVersion.ToSemanticVersion() < minimumCommonLibVersion.ToSemanticVersion())
                {
                    packagesNeedUpdating = true;
                    break;
                }
            }

            return packagesNeedUpdating;
        }

        private string Update(string envFileContents, bool useLinuxBaseImage, string stringToFind, Func<bool, string> getImageVersion)
        {
            var stringToReplace = FindLineToReplace(envFileContents, stringToFind);
            var imageVersion = getImageVersion(useLinuxBaseImage);

            return stringToReplace is null 
                ? envFileContents 
                : envFileContents.Replace(stringToReplace, $"{stringToFind}{imageVersion}");
        }

        private string UpdateSdkImage(string envFileContents, bool useLinuxBaseImage)
        {
            const string dotnetSdkImage = "DOTNET_SDK_IMAGE=microsoft/";

            return Update(envFileContents, useLinuxBaseImage, dotnetSdkImage, GetSdkImageVersion);
        }

        private string UpdateRuntimeImage(string envFileContents, bool useLinuxBaseImage)
        {
            const string dotnetRuntimeImage = "DOTNET_IMAGE=microsoft/";

            return Update(envFileContents, useLinuxBaseImage, dotnetRuntimeImage, GetRuntimeVersion);
        }

        private string FindLineToReplace(string stringContent, string stringToFind)
        {
            var start = stringContent.IndexOf(stringToFind, InvariantCultureIgnoreCase);
            if (start <= -1)//TODO: unit test
                return null;

            var end = stringContent.IndexOf(Environment.NewLine, start, InvariantCultureIgnoreCase);
            return stringContent.Substring(start, end - start);
        }

        private string GetSdkImageVersion(bool useLinuxBaseImage)
        {
            return useLinuxBaseImage ? _dotNetVersionUpdater.LinuxSdkImageVersion : _dotNetVersionUpdater.AlpineSdkImageVersion;
        }

        private string GetRuntimeVersion(bool useLinuxBaseImage)
        {
            return useLinuxBaseImage ? _dotNetVersionUpdater.LinuxRuntimeImageVersion : _dotNetVersionUpdater.AlpineRuntimeImageVersion;
        }
        
        private string UpdateProjectFileContents(string projectFileContents, bool useLinuxBaseImage)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(projectFileContents);
            xmlDoc.PreserveWhitespace = true;

            var xPathNavigator = xmlDoc.CreateNavigator();
            var targetFrameWorkNode = xPathNavigator.SelectSingleNode("/Project/PropertyGroup/TargetFramework");
            var targetFrameWorksNode = xPathNavigator.SelectSingleNode("/Project/PropertyGroup/TargetFrameworks");
            var metaPackageNode = xPathNavigator.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include=\"Microsoft.AspNetCore.All\"]");

            UpdateTargetFramework(targetFrameWorkNode, _dotNetVersionUpdater.TargetFramework);
            UpdateTargetFrameworks(targetFrameWorksNode, _dotNetVersionUpdater.TargetFramework);

            //TODO: unit test this IF
            if (metaPackageNode != null)
            {
                metaPackageNode.OuterXml = "<PackageReference Include=\"Microsoft.AspNetCore.App\" />";
            }

            if (PackagesNeedUpdating(xPathNavigator, _dotNetVersionUpdater.MinimumCommonLibVersion))
            {
                var packages = xPathNavigator.Select(@"/Project/ItemGroup/PackageReference[starts-with(@Include, ""MicroMachines.Common"")]");

                var packageNames = new List<string>();

                while (packages.MoveNext())
                {
                    packageNames.Add(packages.Current.GetAttribute("Include", ""));
                }

                foreach (var packageName in packageNames)
                {
                    var package = xPathNavigator.SelectSingleNode($"/Project/ItemGroup/PackageReference[@Include=\"{packageName}\"]");
                    package.OuterXml = $"<PackageReference Include=\"{packageName}\" Version=\"{_dotNetVersionUpdater.MinimumCommonLibVersion}\" />";
                }
            }

            return xmlDoc.Format();
        }

        private string UpdateEnvFileContent(string envFileContents, bool useLinuxBaseImage)
        {
            var updatedEnvFile = UpdateSdkImage(envFileContents, useLinuxBaseImage);
            updatedEnvFile = UpdateRuntimeImage(updatedEnvFile, useLinuxBaseImage);
            updatedEnvFile = UpdateMonoVersion(updatedEnvFile);

            return updatedEnvFile;
        }

        private string UpdateMonoVersion(string envFileContents)
        {
            const string startPosition = "MONO_IMAGE=mono:";
            var stringToReplace = FindLineToReplace(envFileContents, startPosition);

            return stringToReplace == null ? envFileContents :
                envFileContents.Replace(stringToReplace, $"{startPosition}{_dotNetVersionUpdater.MonoVersion}");
        }

        private string UpdateDockerFileContent(string dockerFileContents, bool useLinuxBaseImage)
        {
            var updatedContent = UpdateFromStatement(dockerFileContents, useLinuxBaseImage);
            return UpdateCopyStatement(updatedContent);
        }

        private string UpdateCopyStatement(string dockerFileContents)
        {
            return dockerFileContents.Replace(_dotNetVersionUpdater.CurrentFramework, _dotNetVersionUpdater.TargetFramework);
        }

        private string UpdateFromStatement(string dockerFileContents, bool useLinuxBaseImage)
        {
            const string fromStatment = "FROM microsoft/";

            var stringToReplace = FindLineToReplace(dockerFileContents, fromStatment);
            var baseImageVersion = GetRuntimeVersion(useLinuxBaseImage);

            //TODO: Unit test required
            return stringToReplace is null
                ? dockerFileContents
                : dockerFileContents.Replace(stringToReplace, $"{fromStatment}{baseImageVersion}");
        }
    }
}