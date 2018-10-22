using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Automatize;
using Automatize.FileFinders;
using NUnit.Framework;

namespace Automatize.Tests.FileFinders
{
    public class ProjectFileFinderTests
    {
        [Test]
        public void ShouldNotFindAnyProjectFiles()
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

        [Test]
        public void ShouldFindAllProjectFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}src{OS.Slash}srcprojectfolder{OS.Slash}srcproject.csproj", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}tests{OS.Slash}testprojectfolder{OS.Slash}testproject.csproj", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}folder{OS.Slash}otherproj.csproj", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results.First().Name, Is.EqualTo("srcproject.csproj"));
            Assert.That(results[1].Name, Is.EqualTo("testproject.csproj"));
            Assert.That(results[2].Name, Is.EqualTo("otherproj.csproj"));
        }

        [Test]
        public void ShouldFindAllProjectFilesTypes()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}src{OS.Slash}srcprojectfolder{OS.Slash}srcproject.csproj", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}tests{OS.Slash}testprojectfolder{OS.Slash}testproject.vbproj", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}other{OS.Slash}random{OS.Slash}folder{OS.Slash}otherproj.fsproj", new MockFileData("") },
                { $"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}unknown{OS.Slash}unknownproj.ukproj", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search($"C:{OS.Slash}dev{OS.Slash}solutionfolder{OS.Slash}").ToList();

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.First().Name, Is.EqualTo("srcproject.csproj"));
            Assert.That(results[1].Name, Is.EqualTo("testproject.vbproj"));
            Assert.That(results[2].Name, Is.EqualTo("otherproj.fsproj"));
            //TODO: at some point, filter out anything other that the above
            Assert.That(results[3].Name, Is.EqualTo("unknownproj.ukproj"));
        }
    }
}