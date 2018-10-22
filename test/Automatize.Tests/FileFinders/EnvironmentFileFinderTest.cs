using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Automatize.FileFinders;
using NUnit.Framework;

namespace Automatize.Tests.FileFinders
{
    public class EnvironmentFileFinderTest
    {
        [Test]
        public void ShouldFindAllEnvironmentFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}src{OS.Slash}srcprojectfolder{OS.Slash}.env", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}tests{OS.Slash}testprojectfolder{OS.Slash}.Env", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}folder{OS.Slash}.ENV", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}test{OS.Slash}test.env", new MockFileData("") }
            });

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.First().Name, Is.EqualTo(".env"));
            Assert.That(results[1].Name, Is.EqualTo(".Env"));
            Assert.That(results[2].Name, Is.EqualTo(".ENV"));
            Assert.That(results[3].Name, Is.EqualTo("test.env"));
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

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}