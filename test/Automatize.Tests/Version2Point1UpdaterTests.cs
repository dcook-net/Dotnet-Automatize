using System.Xml;
using Automatize.VersionUpdaters;
using NUnit.Framework;

namespace Automatize.Tests
{
    public class Version2Point1UpdaterTests : ProjectFileUpdaterTestBase
    {
        private Version2Point1Updater _updater;

        [SetUp]
        public void Setup()
        {
            _updater = new Version2Point1Updater();
        }

        [Test]
        public void ShouldUpdateXmlContentAsExpected()
        {
            var updatedXml = _updater.UpdateProjectFileContents(ReadResourceFile("SampleProjectFile.xml"), false);
            var expectedProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedProjFile.xml"));

            var updatedXmlDoc = ConvertToXmlDoc(updatedXml);

            Assert.That(updatedXmlDoc.OuterXml, Is.EqualTo(expectedProjFile.OuterXml));
        }

        [Test]
        public void ShouldUpdateLibraryProjectFile_HandlingTargetFrameworksNode()
        {
            var updatedXml = _updater.UpdateProjectFileContents(ReadResourceFile("LibProjectFile.xml"), false);
            var expectedProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedLibProjFile.xml"));

            var updatedXmlDoc = ConvertToXmlDoc(updatedXml);

            Assert.That(updatedXmlDoc.OuterXml, Is.EqualTo(expectedProjFile.OuterXml));
        }

        [Test]
        public void ShouldThrowExceptionWhenXmlDocumentIsInvalid()
        {
            Assert.Throws<XmlException>(() => _updater.UpdateProjectFileContents(ReadResourceFile("InvalidXmlFile.xml"), false));
        }

        [Test]
        public void ShouldUpdateDockerFile_AlpineBaseImage()
        {
            var sampleDockerFileContent = ReadResourceFile("SampleDockerFile");
            var expectedDockerFile = ReadResourceFile("ExpectedDockerFile");

            var updatedDockerFile = _updater.UpdateDockerFileContent(sampleDockerFileContent, false);

            Assert.That(updatedDockerFile, Is.EqualTo(expectedDockerFile));
        }

        [Test]
        public void ShouldUpdateDockerFile_LinuxBaseImage()
        {
            var sampleDockerFileContent = ReadResourceFile("SampleDockerFile");
            var expectedDockerFile = ReadResourceFile("ExpectedDockerFile_Linux");

            var updatedDockerFile = _updater.UpdateDockerFileContent(sampleDockerFileContent, true);

            Assert.That(updatedDockerFile, Is.EqualTo(expectedDockerFile));
        }

        [Test]
        public void ShouldUpdateEnvFile_AlpineBaseImage()
        {
            var sampleEnvFileContent = ReadResourceFile("Sample.env");
            var expectedEnvFile = ReadResourceFile("Expected.env");

            var updatedEnvFile = _updater.UpdateEnvFileContent(sampleEnvFileContent, false);

            Assert.That(updatedEnvFile, Is.EqualTo(expectedEnvFile));
        }

        [Test]
        public void ShouldUpdateEnvFile_LinuxBaseImage()
        {
            var sampleEnvFileContent = ReadResourceFile("Sample.env");
            var expectedEnvFile = ReadResourceFile("Expected_Linux.env");

            var updatedEnvFile = _updater.UpdateEnvFileContent(sampleEnvFileContent, true);

            Assert.That(updatedEnvFile, Is.EqualTo(expectedEnvFile));
        }
    }
}