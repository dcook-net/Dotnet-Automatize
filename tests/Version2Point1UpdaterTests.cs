using System.Xml;
using AutoUpgrade.VersionUpdaters;
using NUnit.Framework;

namespace AutoUpgradeTests
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
            var updatedXml = _updater.UpdateProjectFileContents(SampleProjFileXmlContent);

            var updatedXmlDoc = ConvertToXmlDoc(updatedXml);

            Assert.That(updatedXmlDoc.OuterXml, Is.EqualTo(ExpectedProjFile.OuterXml));
        }

        [Test]
        public void ShouldThrowExceptionWhenXmlDocumentIsInvalid()
        {
            Assert.Throws<XmlException>(() => _updater.UpdateProjectFileContents(InvalidProjFileXmlContent));
        }

        [Test]
        public void ShouldUpdateDockerFile()
        {
            var sampleDockerFileContent = ReadResourceFile("SampleDockerFile");
            var expectedDockerFile = ReadResourceFile("ExpectedDockerFile");

            var updatedDockerFile = _updater.UpdateDockerFileContent(sampleDockerFileContent);

            Assert.That(updatedDockerFile, Is.EqualTo(expectedDockerFile));
        }

        [Test]
        public void ShouldUpdateEnvFile()
        {
            var sampleEnvFileContent = ReadResourceFile("Sample.env");
            var expectedEnvFile = ReadResourceFile("Expected.env");

            var updatedEnvFile = _updater.UpdateEnvFileContent(sampleEnvFileContent);

            Assert.That(updatedEnvFile, Is.EqualTo(expectedEnvFile));
        }
    }
}