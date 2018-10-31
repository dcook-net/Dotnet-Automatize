using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Automatize.Extensions;

namespace Automatize.VersionUpdaters
{
    public static class XmlDocumentExtensions
    {
        public static string Format(this XmlDocument doc)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n", //TODO: this is ok for windows, what about nix base OS?
                NewLineHandling = NewLineHandling.Replace
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
    }

    public class Version2Point1Updater : IDotNetVersionUpdater
    {
        private const StringComparison InvariantCultureIgnoreCase = StringComparison.InvariantCultureIgnoreCase;

        public int MajorVersion { get; } = 2;
        public int MinorVersion { get; } = 1;

        public string UpdateProjectFileContents(string projectFileContents, bool useLinuxBaseImage)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(projectFileContents);
            xmlDoc.PreserveWhitespace = true;

            var xPathNavigator = xmlDoc.CreateNavigator();
            var targetFrameWorkNode = xPathNavigator.SelectSingleNode("/Project/PropertyGroup/TargetFramework");
            var targetFrameWorksNode = xPathNavigator.SelectSingleNode("/Project/PropertyGroup/TargetFrameworks");
            var metaPackageNode = xPathNavigator.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include=\"Microsoft.AspNetCore.All\"]");

            UpdateTargetFramework(targetFrameWorkNode);
            UpdateTargetFrameworks(targetFrameWorksNode);

            //TODO: unit test this IF
            if (metaPackageNode != null)
            {
                metaPackageNode.OuterXml = "<PackageReference Include=\"Microsoft.AspNetCore.App\" />";
            }

            if (PackagesNeedUpdating(xPathNavigator))
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
                    package.OuterXml = $"<PackageReference Include=\"{packageName}\" Version=\"{MinimumCommonLibVersion}\" />";
                }
            }

            return xmlDoc.Format();
        }

        private void UpdateTargetFrameworks(XPathNavigator targetFrameWorksNode)
        {
            targetFrameWorksNode?.SetValue(targetFrameWorksNode.Value.Replace("netcoreapp2.0", TargetFramework));
        }

        private void UpdateTargetFramework(XPathNavigator targetFrameWorkNode)
        {
            if (targetFrameWorkNode != null && !targetFrameWorkNode.Value.Contains("netstandard"))
            {
                targetFrameWorkNode.SetValue(TargetFramework);
            }
        }

        private static bool PackagesNeedUpdating(XPathNavigator xPathNavigator)
        {
            var packages = xPathNavigator.Select(@"/Project/ItemGroup/PackageReference[starts-with(@Include, ""MicroMachines.Common"")]");
            var packagesNeedUpdating = false;

            while (packages.MoveNext())
            {
                var packageVersion = packages.Current.GetAttribute("Version", "");

                if (packageVersion.ToSemanticVersion() < MinimumCommonLibVersion.ToSemanticVersion())
                {
                    packagesNeedUpdating = true;
                    break;
                }
            }

            return packagesNeedUpdating;
        }

        public string UpdateEnvFileContent(string envFileContents, bool useLinuxBaseImage)
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
                envFileContents.Replace(stringToReplace, $"{startPosition}{MonoVersion}");
        }

        public string UpdateDockerFileContent(string dockerFileContents, bool useLinuxBaseImage)
        {
            var updatedContent = UpdateFromStatement(dockerFileContents, useLinuxBaseImage);
            return UpdateCopyStatement(updatedContent);
        }

        private static string UpdateSdkImage(string envFileContents, bool useLinuxBaseImage)
        {
            const string dotnetSdkImage = "DOTNET_SDK_IMAGE=microsoft/";
            var stringToReplace = FindLineToReplace(envFileContents, dotnetSdkImage);
            var sdkBaseImageVersion = GetSdkImageVersion(useLinuxBaseImage);

            return stringToReplace == null ? envFileContents : envFileContents.Replace(stringToReplace, $"{dotnetSdkImage}{sdkBaseImageVersion}");
        }

        //TODO: Duplication in these methods! Plus tests
        private static string UpdateRuntimeImage(string envFileContents, bool useLinuxBaseImage)
        {
            const string dotnetRuntimeImage = "DOTNET_IMAGE=microsoft/";
            var stringToReplace = FindLineToReplace(envFileContents, dotnetRuntimeImage);
            var runtimeImageVersion = GetRuntimeVersion(useLinuxBaseImage);

            return stringToReplace == null ? envFileContents : envFileContents.Replace(stringToReplace, $"{dotnetRuntimeImage}{runtimeImageVersion}");
        }

        private static string FindLineToReplace(string stringContent, string stringToFind)
        {
            var start = stringContent.IndexOf(stringToFind, InvariantCultureIgnoreCase);
            if (start <= -1)//TODO: unit test
                return null;

            var end = stringContent.IndexOf(Environment.NewLine, start, InvariantCultureIgnoreCase);
            return stringContent.Substring(start, end - start);
        }

        private string UpdateCopyStatement(string dockerFileContents)
        {
            return dockerFileContents.Replace("netcoreapp2.0", TargetFramework); //TODO: I'd like the 2.0 part to be a bit smarter!
        }

        private static string UpdateFromStatement(string dockerFileContents, bool useLinuxBaseImage)
        {
            const string fromStatment = "FROM microsoft/";

            var stringToReplace = FindLineToReplace(dockerFileContents, fromStatment);
            var baseImageVersion = GetRuntimeVersion(useLinuxBaseImage);

            //TODO: Unit test required
            return stringToReplace == null ? dockerFileContents : dockerFileContents.Replace(stringToReplace, $"{fromStatment}{baseImageVersion}");
        }

        private static string GetSdkImageVersion(bool useLinuxBaseImage)
        {
            return useLinuxBaseImage ? LinuxSdkImageVersion : AlpineSdkImageVersion;
        }

        private static string GetRuntimeVersion(bool useLinuxBaseImage)
        {
            return useLinuxBaseImage ? LinuxRuntimeImageVersion : AlplineRuntimeImageVersion;
        }

        private string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";

        private static string LinuxSdkImageVersion => "dotnet:2.1.403-sdk";
        private static string AlpineSdkImageVersion => "dotnet:2.1.403-sdk-alpine3.7";

        private static string LinuxRuntimeImageVersion => "dotnet:2.1.5-aspnetcore-runtime";
        private static string AlplineRuntimeImageVersion => "dotnet:2.1.5-aspnetcore-runtime-alpine3.7";

        private static string MinimumCommonLibVersion => "1.0.310";

        private static string MonoVersion => "5.14.0.177";
    }
}