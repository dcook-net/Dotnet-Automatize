using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using AutoUpgrade.Extensions;

namespace AutoUpgrade.VersionUpdaters
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

        public string UpdateProjectFileContents(string projectFileContents)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(projectFileContents);
            xmlDoc.PreserveWhitespace = true;

            var xPathNavigator = xmlDoc.CreateNavigator();
            var targetFrameWorkNode = xPathNavigator.SelectSingleNode("/Project/PropertyGroup/TargetFramework");
            var metaPackageNode = xPathNavigator.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include=\"Microsoft.AspNetCore.All\"]");

            if (targetFrameWorkNode != null && !targetFrameWorkNode.Value.Contains("netstandard"))
            {
                targetFrameWorkNode.SetValue(TargetFramework);
            }

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

        public string UpdateEnvFileContent(string envFileContents)
        {
            return UpdateRuntimeImage(UpdateSdkImage(envFileContents));
        }

        public string UpdateDockerFileContent(string dockerFileContents)
        {
            return UpdateCopyStatement(UpdateFromStatement(dockerFileContents));
        }

        private static string UpdateSdkImage(string envFileContents)
        {
            const string dotnetSdkImage = "DOTNET_SDK_IMAGE=microsoft/";
            var stringToReplace = FindLineToReplace(envFileContents, dotnetSdkImage);

            return stringToReplace == null ? envFileContents : envFileContents.Replace(stringToReplace, $"{dotnetSdkImage}{SdkImageVersion}");
        }
        //TODO: Duplication in these methods! Plus tests
        private static string UpdateRuntimeImage(string envFileContents)
        {
            const string dotnetSdkImage = "DOTNET_IMAGE=microsoft/";
            var stringToReplace = FindLineToReplace(envFileContents, dotnetSdkImage);

            return stringToReplace == null ? envFileContents : envFileContents.Replace(stringToReplace, $"{dotnetSdkImage}{RuntimeImageVersion}");
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

        private static string UpdateFromStatement(string dockerFileContents)
        {
            const string fromStatment = "FROM microsoft/";
            var stringToReplace = FindLineToReplace(dockerFileContents, fromStatment);

            //TODO: Unit test required
            return stringToReplace == null ? dockerFileContents : dockerFileContents.Replace(stringToReplace, $"{fromStatment}{RuntimeImageVersion}");
        }

        private string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
        private static string SdkImageVersion => "dotnet:2.1.402-sdk-alpine3.7";
        private static string RuntimeImageVersion => "dotnet:2.1.5-aspnetcore-runtime-alpine3.7";
        private static string MinimumCommonLibVersion => "1.0.309";
    }
}