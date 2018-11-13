using System.Collections.Generic;
using Automatize.Version;
using NUnit.Framework;

namespace Automatize.Tests
{
    [Parallelizable]
    public class VersionUpdaterCollectionTests
    {
        private VersionUpdaterCollection _collection;
        private readonly StubVersionUpdater _onePointZeroUpdater = new StubVersionUpdater { MajorVersion = 1, MinorVersion = 0 };
        private readonly StubVersionUpdater _onePointOneUpdater = new StubVersionUpdater { MajorVersion = 1, MinorVersion = 1 };
        private readonly StubVersionUpdater _twoPointZeroUpdater = new StubVersionUpdater { MajorVersion = 2, MinorVersion = 0 };
        private readonly StubVersionUpdater _twoPointOneUpdater = new StubVersionUpdater { MajorVersion = 2, MinorVersion = 1 };

        public class StubVersionUpdater : IDotNetVersionUpdater
        {
            public int MajorVersion { get; set; }
            public int MinorVersion { get; set; }
            public string CurrentFramework { get; set; }
            public string TargetFramework { get; set; }
            public string MinimumCommonLibVersion { get; set; }
            public string LinuxSdkImageVersion { get; set; }
            public string AlpineSdkImageVersion { get; set; }
            public string LinuxRuntimeImageVersion { get; set; }
            public string AlplineRuntimeImageVersion { get; set; }
            public string MonoVersion { get; set; }
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _collection = new VersionUpdaterCollection(
                new List<IDotNetVersionUpdater>
                {
                    _onePointZeroUpdater,
                    _onePointOneUpdater,
                    _twoPointZeroUpdater,
                    _twoPointOneUpdater
                });
        }

        [Test]
        public void ShouldReturnRequestedVersionUpdater()
        {
            Assert.That(_collection.Find(2, 0), Is.EqualTo(_twoPointZeroUpdater));
        }

        [Test]
        public void ShouldThrowWhenRequestedVersionUpdaterDoesNotExist()
        {
            Assert.Throws<UnsupportedVersionException>(() => _collection.Find(0, 0));
        }

        [Test]
        public void ShouldReturnLatestVersion()
        {
            Assert.That(_collection.GetLatest(), Is.EqualTo(_twoPointOneUpdater));
        }
    }
}