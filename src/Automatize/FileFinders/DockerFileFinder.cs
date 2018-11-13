using System.IO.Abstractions;

namespace Automatize.FileFinders
{
    public class DockerFileFinder : FileFinder, IDockerFileFinder
    {
        public DockerFileFinder(IFileSystem fileSystem = null) 
            : base("DockerFile?*", fileSystem ?? new FileSystem())
        {}
    }
}