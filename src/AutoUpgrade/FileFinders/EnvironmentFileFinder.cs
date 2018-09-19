using System.IO.Abstractions;

namespace AutoUpgrade.FileFinders
{
    public class EnvironmentFileFinder : FileFinder
    {
        public EnvironmentFileFinder(IFileSystem fileSystem) : base(fileSystem, "*.env")
        { }
    }
}