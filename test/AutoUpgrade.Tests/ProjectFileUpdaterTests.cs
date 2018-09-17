using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrade.VersionUpdaters;
using McMaster.Extensions.CommandLineUtils;
using NUnit.Framework;
using static AutoUpgrade.OS;

namespace AutoUpgrade.Tests
{
    public class ProjectFileUpdaterTests : ProjectFileUpdaterTestBase
    {
        private FileUpdater _updater;
        private IDotNetVersionUpdater _version2Point1Updater;
        private PhysicalConsole _console;

        [SetUp]
        public void Setup()
        {
            _console = new PhysicalConsole();
            _version2Point1Updater = new Version2Point1Updater();
            _updater = new FileUpdater(MockFileSystem, _console);
        }
        
        [Test]
        public void ShouldUpdateSuppliedFileWithExpectedChanges()
        {
            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}sampleProjFile.xml", new MockFileData(SampleProjFileXmlContent) },
                { $"C:{Slash}dev{Slash}InvalidXmlFile.xml", new MockFileData(InvalidProjFileXmlContent) }
            });

            _updater = new FileUpdater(MockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}sampleProjFile.xml")
            };

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles.First()));
            
            Assert.That(updatedXmlDoc, Is.EqualTo(ExpectedProjFile));
        }

        [Test]
        public void ShouldLogErrorsAndContinue()
        {
            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}InvalidXmlFile.xml"),
                new FileInfo($"C:{Slash}dev{Slash}sampleProjFile.xml")
            };

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles[1]));

            Assert.That(updatedXmlDoc, Is.EqualTo(ExpectedProjFile));

            //            _console.Out.WriteLine();
            
//            Assert.That(_console.Out.ReadToEnd(), Is.EqualTo("Updating C:\\dev\\InvalidXmlFile.xml failed: File is not valid Xml"));
        }

        [Test]
        public void ShouldNotUpdateTargetFrameworkWhenTargetingNetStandard()
        {
            MockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}NetStandardXmlFile.xml", new MockFileData(ReadResourceFile("NetStandardXmlFile.xml")) }
            });

            _updater = new FileUpdater(MockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}NetStandardXmlFile.xml")
            };

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles[0]));
            var expectedNetStandardProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedNetStandardXmlFile.xml"));

            Assert.That(updatedXmlDoc, Is.EqualTo(expectedNetStandardProjFile));
        }

        [Test, Ignore("Need to figure out how to test this")]//TODO:
        public void ShouldLogWhenListOfFilesIsEmpty()
        {
            _updater.UpdateProjectFiles(new List<FileInfo>(), _version2Point1Updater);

            AssertConsoleOutput();
        }

        [Test, Ignore("Need to figure out how to test this")]//TODO:
        public void ShouldLogWhenListOfFilesIsNull()
        {
            _updater.UpdateProjectFiles(null, _version2Point1Updater);

            AssertConsoleOutput();
        }

        private void AssertConsoleOutput()
        {
            Assert.That(_console.Out, Is.EqualTo("No Project files found!"));
        }
    }
}
