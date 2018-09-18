//using System.Collections.Generic;
//using System.IO.Abstractions.TestingHelpers;
//using System.Linq;
//using AutoUpgrade.FileFinders;
//using NUnit.Framework;
//using static AutoUpgrade.OS;
//
//namespace AutoUpgrade.Tests.FileFinders
//{
//    public class ProjectFileFinderTests
//    {
//        [Test]
//        public void ShouldNotFindAnyProjectFiles()
//        {
//            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
//            {
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}srcproject.txt", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}testproject.cs", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}otherproj.vb", new MockFileData("") }
//            });
//
//            var fileFinder = new ProjectFileFinder(mockFileSystem);
//
//            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();
//
//            Assert.That(results.Count, Is.EqualTo(0));
//        }
//
//        [Test]
//        public void ShouldFindAllProjectFilesInSpecifiedDirectory()
//        {
//            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
//            {
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}srcproject.csproj", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}testproject.csproj", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}otherproj.csproj", new MockFileData("") }
//            });
//
//            var fileFinder = new ProjectFileFinder(mockFileSystem);
//
//            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();
//
//            Assert.That(results.Count, Is.EqualTo(3));
//            Assert.That(results.First().Name, Is.EqualTo("srcproject.csproj"));
//            Assert.That(results[1].Name, Is.EqualTo("testproject.csproj"));
//            Assert.That(results[2].Name, Is.EqualTo("otherproj.csproj"));
//        }
//
//        [Test]
//        public void ShouldFindAllProjectFilesTypes()
//        {
//            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
//            {
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}src{Slash}srcprojectfolder{Slash}srcproject.csproj", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}tests{Slash}testprojectfolder{Slash}testproject.vbproj", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}other{Slash}random{Slash}folder{Slash}otherproj.fsproj", new MockFileData("") },
//                { $"C:{Slash}dev{Slash}solutionfolder{Slash}unknown{Slash}unknownproj.ukproj", new MockFileData("") }
//            });
//
//            var fileFinder = new ProjectFileFinder(mockFileSystem);
//
//            var results = fileFinder.Search($"C:{Slash}dev{Slash}solutionfolder{Slash}").ToList();
//
//            Assert.That(results.Count, Is.EqualTo(4));
//            Assert.That(results.First().Name, Is.EqualTo("srcproject.csproj"));
//            Assert.That(results[1].Name, Is.EqualTo("testproject.vbproj"));
//            Assert.That(results[2].Name, Is.EqualTo("otherproj.fsproj"));
//            //TODO: at some point, filter out anything other that the above
//            Assert.That(results[3].Name, Is.EqualTo("unknownproj.ukproj"));
//        }
//    }
//}