using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrade.FileFinders;
using NUnit.Framework;
using static AutoUpgrade.OS;

namespace AutoUpgrade.Tests.FileFinders
{
    public class DockerFileFinderTests
    {
        [Test]
        public void ShouldFindAllDockerFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}DockerFile", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}dockerfile", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}dockerFile", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}goals{Slash}DOCKERFILE", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}test{Slash}DockerFile.test", new MockFileData("") }
            });

            var fileFinder = new DockerFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();

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
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}srcproject.txt", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}testproject.cs", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}otherproj.vb", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}