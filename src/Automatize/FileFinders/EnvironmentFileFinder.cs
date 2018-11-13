using System.IO.Abstractions;

namespace Automatize.FileFinders
{
    public class EnvironmentFileFinder : FileFinder, IEnvironmentFileFinder
    {
        public EnvironmentFileFinder(IFileSystem fileSystem = null) 
            : base("*.env", fileSystem ?? new FileSystem())
        { }
    }
}