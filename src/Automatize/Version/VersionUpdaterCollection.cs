using System.Collections.Generic;
using System.Linq;

namespace Automatize.Version
{
    public class VersionUpdaterCollection : IVersionUpdateCollection
    {
        private readonly List<IDotNetVersionUpdater> _versionUpdaters;

        public VersionUpdaterCollection(IEnumerable<IDotNetVersionUpdater> versionUpdaters)
        {
            _versionUpdaters = versionUpdaters.OrderBy(x => x.MajorVersion)
                                              .ThenBy(x => x.MinorVersion).ToList();
        }

        public IDotNetVersionUpdater GetLatest() => _versionUpdaters.Last(); //TODO: What if there are none!

        public IDotNetVersionUpdater Find(int major, int minor)
        {
            var match = _versionUpdaters.Find(x => x.MajorVersion == major && 
                                                     x.MinorVersion == minor);

            if (match is null)
                throw new UnsupportedVersionException(major, minor);

            return match;
        }
    }
}