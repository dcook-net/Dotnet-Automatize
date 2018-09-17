using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrader.FileFinders;
using NUnit.Framework;

namespace AutoUpgraderTests.FileFinders
{
    public class ProjectFileFinderTests
    {
        [Test]
        public void ShouldNotFindAnyProjectFiles()
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

        [Test]
        public void ShouldFindAllProjectFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\dev\solutionfolder\src\srcprojectfolder\srcproject.csproj", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\testproject.csproj", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\otherproj.csproj", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

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
                { @"C:\dev\solutionfolder\src\srcprojectfolder\srcproject.csproj", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\testproject.vbproj", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\otherproj.fsproj", new MockFileData("") },
                { @"C:\dev\solutionfolder\unknown\unknownproj.ukproj", new MockFileData("") }
            });

            var fileFinder = new ProjectFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

            Assert.That(results.Count, Is.EqualTo(4));
            Assert.That(results.First().Name, Is.EqualTo("srcproject.csproj"));
            Assert.That(results[1].Name, Is.EqualTo("testproject.vbproj"));
            Assert.That(results[2].Name, Is.EqualTo("otherproj.fsproj"));
            //TODO: at some point, filter out anything other that the above
            Assert.That(results[3].Name, Is.EqualTo("unknownproj.ukproj"));
        }
    }
}