using System.IO.Abstractions;

namespace AutoUpgrader.FileFinders
{
    public class DockerFileFinder : FileFinder
    {
        public DockerFileFinder(IFileSystem fileSystem) : base(fileSystem, "DockerFile?*")
        {}
    }
}