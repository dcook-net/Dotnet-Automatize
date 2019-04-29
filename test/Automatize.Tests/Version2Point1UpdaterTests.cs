using Automatize.Version;
using NUnit.Framework;

namespace Automatize.Tests
{
    [Parallelizable]
    public class VersionUpdaterTests : VersionUpdaterTestBase
    {
        public VersionUpdaterTests() : base(new Version2Point1Updater())
        {}

        [TestCase("SampleProjectFile-DontUpdateCommonLibVersions.csproj", "ExpectedResult-DontUpdateCommonLibVersions.csproj")]
        [TestCase("SampleProjectFile.csproj", "ExpectedProjFile.csproj")]
        [TestCase("NetStandardXmlFile.csproj", "ExpectedNetStandardXmlFile.csproj")]
        [TestCase("LibProjectFile.csproj", "ExpectedLibProjFile.csproj", true)]
        public void ShouldUpGradeProjectFile(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool isLibraryProject = false)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, false, isLibraryProject);
        }

        [TestCase("DockerFile_Sample", "DockerFile_Expected", false)]
        [TestCase("DockerFile_Sample", "DockerFile_Linux_Expected", true)]
        public void ShouldUpgradeDockerFile(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool useLinuxBaseImage)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, useLinuxBaseImage, false);
        }

        [TestCase("Sample.env", "Expected.env", false, false)]
        [TestCase("Sample.env", "Expected_Linux.env", true, false)]
        [TestCase("Sample.env", "Expected_Linux_Library.env", true, true)]
        [TestCase("Sample.env", "Expected_Library.env", false, true)]
        public void ShouldUpgradeEnvironmentFiles(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool useLinuxBaseImage, bool isLibraryProject)
        {
            TestUpgrade(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent, useLinuxBaseImage, isLibraryProject);
        }

        [TestCase]
        public void ShouldThrowExceptionWhenXmlDocumentIsInvalid()
        {
            TestUpgrade("InvalidXmlFile.csproj", "InvalidXmlFile.csproj", false, false);
        }
    }
}