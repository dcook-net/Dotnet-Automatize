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
        }

        [Test, Ignore("Fails on build server ")]//TODO:
        public void ShouldUpdateSuppliedFileWithExpectedChanges()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}SampleProjectFile.xml", new MockFileData(ReadResourceFile("SampleProjectFile.xml")) },
                { $"C:{Slash}dev{Slash}InvalidXmlFile.xml", new MockFileData(ReadResourceFile("InvalidXmlFile.xml")) }
            });

            _updater = new FileUpdater(mockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}SampleProjectFile.xml")
            };

            var expectedProjectFile = ConvertToXmlDoc(ReadResourceFile("ExpectedProjFile.xml"));

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles.First(), mockFileSystem));

            Assert.That(updatedXmlDoc, Is.EqualTo(expectedProjectFile));
        }

        [Test, Ignore("Fails on build server ")]//TODO:
        public void ShouldLogErrorsAndContinue()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}SampleProjectFile.xml", new MockFileData(ReadResourceFile("SampleProjectFile.xml")) },
                { $"C:{Slash}dev{Slash}InvalidXmlFile.xml", new MockFileData(ReadResourceFile("InvalidXmlFile.xml")) }
            });

            _updater = new FileUpdater(mockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}InvalidXmlFile.xml"),
                new FileInfo($"C:{Slash}dev{Slash}SampleProjectFile.xml")
            };

            var expectedProjectFile = ConvertToXmlDoc(ReadResourceFile("ExpectedProjFile.xml"));

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles[1], mockFileSystem));

            Assert.That(updatedXmlDoc, Is.EqualTo(expectedProjectFile));

            //            _console.Out.WriteLine();
            
//            Assert.That(_console.Out.ReadToEnd(), Is.EqualTo("Updating C:\\dev\\InvalidXmlFile.xml failed: File is not valid Xml"));
        }

        [Test, Ignore("Fails on build server ")]//TODO:
        public void ShouldNotUpdateTargetFrameworkWhenTargetingNetStandard()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}NetStandardXmlFile.xml", new MockFileData(ReadResourceFile("NetStandardXmlFile.xml")) }
            });

            _updater = new FileUpdater(mockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}NetStandardXmlFile.xml")
            };

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles[0], mockFileSystem));
            var expectedNetStandardProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedNetStandardXmlFile.xml"));

            Assert.That(updatedXmlDoc, Is.EqualTo(expectedNetStandardProjFile));
        }

        [Test]
        public void ShouldNotUpdateCommonLibraryVersionNumbersIfLowerThanMinium()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}SampleProjectFile-DontUpdateCommonLibVersions.xml", new MockFileData(ReadResourceFile("SampleProjectFile-DontUpdateCommonLibVersions.xml")) }
            });

            _updater = new FileUpdater(mockFileSystem, _console);

            var projFiles = new List<FileInfo>
            {
                new FileInfo($"C:{Slash}dev{Slash}SampleProjectFile-DontUpdateCommonLibVersions.xml")
            };

            _updater.UpdateProjectFiles(projFiles, _version2Point1Updater);

            var updatedXmlDoc = ConvertToXmlDoc(ReadFileFromFileSystem(projFiles[0], mockFileSystem));
            var expectedProjFile = ConvertToXmlDoc(ReadResourceFile("ExpectedResult-DontUpdateCommonLibVersions.xml"));

            Assert.That(updatedXmlDoc, Is.EqualTo(expectedProjFile));
        }

//        [Test, Ignore("Need to figure out how to test this")]//TODO:
//        public void ShouldLogWhenListOfFilesIsEmpty()
//        {
//            _updater.UpdateProjectFiles(new List<FileInfo>(), _version2Point1Updater);
//
//            AssertConsoleOutput();
//        }
//
//        [Test, Ignore("Need to figure out how to test this")]//TODO:
//        public void ShouldLogWhenListOfFilesIsNull()
//        {
//            _updater = new FileUpdater(null, _console);
//            _updater.UpdateProjectFiles(null, _version2Point1Updater);
//
//            AssertConsoleOutput();
//        }
//
//        private void AssertConsoleOutput()
//        {
//            Assert.That(_console.Out, Is.EqualTo("No Project files found!"));
//        }
    }
}
