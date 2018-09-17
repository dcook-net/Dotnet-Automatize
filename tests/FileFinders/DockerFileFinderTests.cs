using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrade.FileFinders;
using NUnit.Framework;

namespace AutoUpgradeTests.FileFinders
{
    public class DockerFileFinderTests
    {
        [Test]
        public void ShouldFindAllDockerFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\dev\solutionfolder\src\srcprojectfolder\DockerFile", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\dockerfile", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\dockerFile", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\goals\DOCKERFILE", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\test\DockerFile.test", new MockFileData("") }
            });

            var fileFinder = new DockerFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

            Assert.That(results.Count, Is.EqualTo(5));
            Assert.That(results.First().Name, Is.EqualTo("DockerFile"));
            Assert.That(results[1].Name, Is.EqualTo("dockerfile"));
            Assert.That(results[2].Name, Is.EqualTo("dockerFile"));
            Assert.That(results[3].Name, Is.EqualTo("DOCKERFILE"));
            Assert.That(results[4].Name, Is.EqualTo("DockerFile.test"));
        }

        [Test]
        public void ShouldNotFindAnyDockerFiles()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\dev\solutionfolder\src\srcprojectfolder\srcproject.txt", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\testproject.cs", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\otherproj.vb", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}