//using System.IO.Abstractions.TestingHelpers;
//using Automatize.FileFinders;
//using McMaster.Extensions.CommandLineUtils;
//using NUnit.Framework;
//
//namespace Automatize.Tests
//{
//    [Parallelizable]
//    public class UpdaterFactoryTests
//    {
//        [Test]
//        public void foo()
//        {
//            var mockFileSystem = new MockFileSystem();
//            var factory = new UpdaterFactory(mockFileSystem, 
//                                                new PhysicalConsole(), 
//                                                new ProjectFileFinder(mockFileSystem),
//                                                new DockerFileFinder(mockFileSystem),
//                                                new EnvironmentFileFinder(mockFileSystem));
//
//            var updater = factory.Build(new VersionUpdaterCollectionTests.StubVersionUpdater());
//
//            Assert.That(updater.);
//        }
//    }
//}