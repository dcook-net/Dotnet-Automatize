using System.IO.Abstractions;

namespace Automatize.FileFinders
{
    public class DockerComposeFileFinder : FileFinder, IDockerComposeFileFinder
    {
        public DockerComposeFileFinder(IFileSystem fileSystem = null) 
            : base("*compose*.yml", fileSystem ?? new FileSystem())
        {}
    }
}