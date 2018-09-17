using System.IO.Abstractions;

namespace AutoUpgrader.FileFinders
{
    public class EnvironmentFileFinder : FileFinder
    {
        public EnvironmentFileFinder(IFileSystem fileSystem) : base(fileSystem, "*.env")
        { }
    }
}