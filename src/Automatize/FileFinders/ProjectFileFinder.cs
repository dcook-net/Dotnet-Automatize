using System.IO.Abstractions;

namespace Automatize.FileFinders
{
    public class ProjectFileFinder : FileFinder, IProjectFileFinder
    {
        //TODO: Might be a good idea to discover project files from the solution file?
        //Craven had an idea that you could run the tool, pointing it at a sol file or a proj file
        //However, that wouldn't reference the docker files or the Env File....one to think on
        public ProjectFileFinder(IFileSystem fileSystem = null) 
            : base("*.??proj", fileSystem ?? new FileSystem())
        {}
    }
}