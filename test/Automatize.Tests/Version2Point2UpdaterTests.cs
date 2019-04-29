using Automatize.Version;
using NUnit.Framework;

namespace Automatize.Tests
{
    [Parallelizable]
    public class Version2Point2UpdaterTests : VersionUpdaterTestBase
    {
        public Version2Point2UpdaterTests() : base(new Version2Point2Updater())
        {}

        //TODO: Question the validity of the DontUpdateCommonLibVersions test case, as it seems to upgrade the version of common lib!
        //Dont know why it would not do this
        [TestCase("SampleProjectFile-DontUpdateCommonLibVersions-v2.1.csproj", "ExpectedResult-DontUpdateCommonLibVersions-v2.2.4.csproj")]
        [TestCase("SampleProjectFile-v2.1.csproj", "ExpectedProjFile-v2.2.4.csproj")]
        [TestCase("NetStandardXmlFile.csproj", "ExpectedNetStandardXmlFile.csproj")]
        [TestCase("LibProjectFile-v2.1.csproj", "ExpectedLibProjFile-v2.2.4.csproj", true)]
        public void ShouldUpGradeProjectFile(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool isLibraryProject = false)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, false, isLibraryProject);
        }

        [TestCase("DockerFile_Sample-v2.1", "DockerFile_Expected-v2.2.4", false)]
        [TestCase("DockerFile_Sample-v2.1", "DockerFile_Linux_Expected-v2.2.4", true)]
        [TestCase("DockerFile_Sample-v2.1-mcr", "DockerFile_Expected-v2.2.4", false)]
        [TestCase("DockerFile_Sample-v2.1-mcr", "DockerFile_Linux_Expected-v2.2.4", true)]
        public void ShouldUpgradeDockerFile(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool useLinuxBaseImage)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, useLinuxBaseImage, false);
        }

        [TestCase("Sample.env", "Expected-v2.2.4.env", false, false)]
        [TestCase("Sample-mrc.env", "Expected-v2.2.4.env", false, false)]
        [TestCase("Sample.env", "Expected_Linux-v2.2.4.env", true, false)]
        [TestCase("Sample-mrc.env", "Expected_Linux-v2.2.4.env", true, false)]
        [TestCase("Sample.env", "Expected-Library-v2.2.4.env", false, true)]
        [TestCase("Sample-mrc.env", "Expected-Library-v2.2.4.env", false, true)]
        [TestCase("Sample.env", "Expected-Library_Linux-v2.2.4.env", true, true)]
        [TestCase("Sample-mrc.env", "Expected-Library_Linux-v2.2.4.env", true, true)]
        public void ShouldUpgradeEnvironmentFiles(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool useLinuxBaseImage, bool isLibraryProject)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, useLinuxBaseImage, isLibraryProject);
        }

        [TestCase]
        public void ShouldThrowExceptionWhenXmlDocumentIsInvalid()
        {
            TestUpgrade("InvalidXmlFile.csproj", "InvalidXmlFile.csproj", false, false);
        }

        [TestCase("sample-docker-compose.yml", "expected-docker-compose-v2.2.yml")]
        public void ShouldUpdateDockerComposeFile(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, false, false);
        }
    }
}