using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using Automatize.FileFinders;
using Automatize.Version;
using McMaster.Extensions.CommandLineUtils;
using NUnit.Framework;

namespace Automatize.Tests
{
    public abstract class VersionUpdaterTestBase
    {
        private readonly IDotNetVersionUpdater _dotNetVersionUpdater; 
        private MockFileSystem _mockFileSystem;
        private readonly string _root = $"C:{OS.Slash}dev{OS.Slash}";

        protected VersionUpdaterTestBase(IDotNetVersionUpdater dotNetVersionUpdater)
        {
            _dotNetVersionUpdater = dotNetVersionUpdater;
        }

        protected static Assembly GetExecutingAssembly => Assembly.GetExecutingAssembly();
        
        protected static string ReadResourceFile(string resourcesFileName)
        {
            var assembly = GetExecutingAssembly;
            string sampleDocument;
            var resourceName = ResourceFiles(assembly).Find(t => t.EndsWith(resourcesFileName));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                sampleDocument = reader.ReadToEnd();
            }

            return sampleDocument;
        }

        protected static List<string> ResourceFiles(Assembly assembly)
        {
            return assembly
                .GetManifestResourceNames()
                .ToList();
        }

        protected void TestUpgrade(string nameOfResourceFileWithContentToUpdate, string nameOfResourceFileWithExpectedContent, bool useLinuxBaseImage)
        {
            SetupMockeFileSystem(nameOfResourceFileWithContentToUpdate);
            var versionUpdater = CreateVersionUpdater(_mockFileSystem);

            versionUpdater.UpgradeToVersion(_root, useLinuxBaseImage);

            AssertFileContentAsExpected(nameOfResourceFileWithContentToUpdate, nameOfResourceFileWithExpectedContent);
        }

        private Updater CreateVersionUpdater(IFileSystem mockFileSystem)
        {
            var projectFileFileFinder = new ProjectFileFinder(mockFileSystem);
            var dockerFileFileFinder = new DockerFileFinder(mockFileSystem);
            var environmentFileFileFinder = new EnvironmentFileFinder(mockFileSystem);

            return new Updater(_dotNetVersionUpdater, new PhysicalConsole(), projectFileFileFinder,
                dockerFileFileFinder, environmentFileFileFinder, mockFileSystem);
        }

        private void AssertFileContentAsExpected(string nameOfFileInMockFileSystem, string nameOfResourceFileWithExpecteContent)
        {
            var fileContent = GetFileContent(nameOfFileInMockFileSystem);
            Assert.That(fileContent, Is.EqualTo(ReadResourceFile(nameOfResourceFileWithExpecteContent)));
        }

        private void SetupMockeFileSystem(string nameOfResourceFileContainingContent)
        {
            var fileName = $"{_root}{nameOfResourceFileContainingContent}";

            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {fileName, new MockFileData(ReadResourceFile(nameOfResourceFileContainingContent))}
            });
        }

        private string GetFileContent(string fileName) => _mockFileSystem.File.ReadAllText($"{_root}{fileName}");
    }
}