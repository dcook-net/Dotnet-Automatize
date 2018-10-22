using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Automatize.FileFinders;
using NUnit.Framework;

namespace Automatize.Tests.FileFinders
{
    public class DockerFileFinderTests
    {
        [Test]
        public void ShouldFindAllDockerFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}src{OS.Slash}srcprojectfolder{OS.Slash}DockerFile", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}tests{OS.Slash}testprojectfolder{OS.Slash}dockerfile", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}folder{OS.Slash}dockerFile", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}goals{OS.Slash}DOCKERFILE", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}test{OS.Slash}DockerFile.test", new MockFileData("") }
            });

            var fileFinder = new DockerFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

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
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}src{OS.Slash}srcprojectfolder{OS.Slash}srcproject.txt", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}tests{OS.Slash}testprojectfolder{OS.Slash}testproject.cs", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}folder{OS.Slash}otherproj.vb", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}