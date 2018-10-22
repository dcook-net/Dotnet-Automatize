using System.IO.Abstractions;

namespace Automatize.FileFinders
{
    public class DockerFileFinder : FileFinder
    {
        public DockerFileFinder(IFileSystem fileSystem) : base(fileSystem, "DockerFile?*")
        {}
    }
}