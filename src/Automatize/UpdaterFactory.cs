using System.IO.Abstractions;
using Automatize.FileFinders;
using Automatize.Version;
using McMaster.Extensions.CommandLineUtils;

namespace Automatize
{
    public class UpdaterFactory : IUpdaterFacotry
    {
        private readonly IProjectFileFinder _projectFileFinder;
        private readonly IDockerFileFinder _dockerFileFinder;
        private readonly IEnvironmentFileFinder _environmentFileFinder;
        private readonly IFileSystem _fileSystem;
        private readonly IConsole _console;

        public UpdaterFactory(IConsole console,
            IProjectFileFinder projectFileFinder,
            IDockerFileFinder dockerFileFinder,
            IEnvironmentFileFinder environmentFileFinder, 
            IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _console = console;
            _projectFileFinder = projectFileFinder;
            _dockerFileFinder = dockerFileFinder;
            _environmentFileFinder = environmentFileFinder;
        }

        public Updater Build(IDotNetVersionUpdater dotNetVersionUpdater)
        {
            return new Updater(dotNetVersionUpdater, _console, _projectFileFinder, _dockerFileFinder, _environmentFileFinder, _fileSystem);
        }
    }
}