using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using AutoUpgrade.FileFinders;
using NUnit.Framework;

namespace AutoUpgrade.Tests.FileFinders
{
    public class EnvironmentFileFinderTest
    {
        [Test]
        public void ShouldFindAllEnvironmentFilesInSpecifiedDirectory()
        {
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\dev\solutionfolder\src\srcprojectfolder\.env", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\.Env", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\.ENV", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\test\test.env", new MockFileData("") }
            });

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

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
                { @"C:\dev\solutionfolder\src\srcprojectfolder\srcproject.txt", new MockFileData("") },
                { @"C:\dev\solutionfolder\tests\testprojectfolder\testproject.cs", new MockFileData("") },
                { @"C:\dev\solutionfolder\other\random\folder\otherproj.vb", new MockFileData("") }
            });

            var fileFinder = new EnvironmentFileFinder(mockFileSystem);

            var results = fileFinder.Search(@"C:\dev\solutionfolder\").ToList();

            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}