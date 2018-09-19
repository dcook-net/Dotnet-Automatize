using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrade.FileFinders;
using NUnit.Framework;
using static AutoUpgrade.OS;

namespace AutoUpgrade.Tests.FileFinders
{
    public class EnvironmentFileFinderTest
    {
        [Test]
        public void ShouldFindAllEnvironmentFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}.env", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}.Env", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}.ENV", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}test{Slash}test.env", new MockFileData("") }
            });

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();

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
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}srcproject.txt", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}testproject.cs", new MockFileData("") },
                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}otherproj.vb", new MockFileData("") }
            });

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}