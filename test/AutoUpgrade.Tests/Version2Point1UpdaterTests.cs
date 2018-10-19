using System.Xml;
using AutoUpgrade.VersionUpdaters;
using NUnit.Framework;

namespace AutoUpgrade.Tests
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
            var updatedXml = _updater.UpdateProjectFileContents(ReadResourceFile("SampleProjectFile.xml"));
            var expectedProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedProjFile.xml"));

            var updatedXmlDoc = ConvertToXmlDoc(updatedXml);

            Assert.That(updatedXmlDoc.OuterXml, Is.EqualTo(expectedProjFile.OuterXml));
        }

        [Test]
        public void ShouldThrowExceptionWhenXmlDocumentIsInvalid()
        {
            Assert.Throws<XmlException>(() => _updater.UpdateProjectFileContents(ReadResourceFile("InvalidXmlFile.xml")));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("unknown")]
        [TestCase(Program.DefaultBaseImage)]
        public void ShouldUpdateDockerFile_Alpine(string baseImage)
        {
            var sampleDockerFileContent = ReadResourceFile("SampleDockerFile");
            var expectedDockerFile = ReadResourceFile("ExpectedDockerFile");

            var updatedDockerFile = _updater.UpdateDockerFileContent(sampleDockerFileContent, baseImage);

            Assert.That(updatedDockerFile, Is.EqualTo(expectedDockerFile));
        }

        [Test]
        public void ShouldUpdateDockerFile_NotAlpine()
        {
            var sampleDockerFileContent = ReadResourceFile("SampleDockerFile");
            var expectedDockerFile = ReadResourceFile("ExpectedDockerFile_Linux");

            var updatedDockerFile = _updater.UpdateDockerFileContent(sampleDockerFileContent, "Linux");

            Assert.That(updatedDockerFile, Is.EqualTo(expectedDockerFile));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("unknown")]
        [TestCase(Program.DefaultBaseImage)]
        public void ShouldUpdateEnvFile(string baseImage)
        {
            var sampleEnvFileContent = ReadResourceFile("Sample.env");
            var expectedEnvFile = ReadResourceFile("Expected.env");

            var updatedEnvFile = _updater.UpdateEnvFileContent(sampleEnvFileContent);

            Assert.That(updatedEnvFile, Is.EqualTo(expectedEnvFile));
        }

        [Test]
        public void ShouldUpdateEnvFile_NotAlpine()
        {
            var sampleEnvFileContent = ReadResourceFile("Sample.env");
            var expectedEnvFile = ReadResourceFile("Expected_Linux.env");

            var updatedEnvFile = _updater.UpdateEnvFileContent(sampleEnvFileContent, "Linux");

            Assert.That(updatedEnvFile, Is.EqualTo(expectedEnvFile));
        }
    }
}