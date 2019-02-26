using System.Collections.Generic;
using System.IO;

namespace Automatize.FileFinders
{
    public interface IFileFinder
    {
        IEnumerable<FileInfo> Search(string directoryToSearch);
    }

    //TODO: remove these interfaces and just use the one?

    public interface IProjectFileFinder : IFileFinder
    {
    }

    public interface IDockerFileFinder : IFileFinder
    {
    }

    public interface IEnvironmentFileFinder : IFileFinder
    {
    }
    
    public interface IDockerComposeFileFinder : IFileFinder
    {
    }
}