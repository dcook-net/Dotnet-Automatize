using System.IO.Abstractions;

namespace AutoUpgrade.FileFinders
{
    public class DockerFileFinder : FileFinder
    {
        public DockerFileFinder(IFileSystem fileSystem) : base(fileSystem, "DockerFile?*")
        {}
    }
}