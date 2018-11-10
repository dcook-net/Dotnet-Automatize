using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Automatize.FileFinders;
using Automatize.VersionUpdater;
using McMaster.Extensions.CommandLineUtils;

namespace Automatize
{
    [Command(
        Description = "Searches the specified directory and upgrades various files to the requested version of .Net",
        Name = "dotnet Automatize",
        FullName = "dotnet-Automatize")]
    [HelpOption]
    public class UpgradeCommand
    {
        [Argument(0, ShowInHelpText = true, Name = "path", Description = "Path to the directory to upgrade. Defaults to current location.")]
        [DirectoryExists]
        public string Path { get; private set; }

        [Option(CommandOptionType.NoValue, ShowInHelpText = true, ShortName = "l", LongName = "useLinux",
            Description = "The Base image to use in your DockerFile. Valid options are 'Alpine' or 'Linux'")]
        public bool UseLinuxBaseImage { get; }

        private readonly IFileSystem _fileSystem;

        public UpgradeCommand() : this(null)
        {}

        public UpgradeCommand(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                Path = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(Path))
            {
                console.WriteLine("Supplied Path does not exist. Aborting");
                return await Task.FromResult(-1);
            }

            console.WriteLine($"Starting updating of {Path}");

            var projectFileFileFinder = new ProjectFileFinder(_fileSystem);
            var dockerFileFinder = new DockerFileFinder(_fileSystem);
            var environmentFileFinder = new EnvironmentFileFinder(_fileSystem);

            //TODO: inject the version updater based on the version the user wants
            var dotNetVersionUpdater = new Version2Point1Updater();

            var versionUpdater = new VersionUpdater.VersionUpdater(dotNetVersionUpdater,
                                                    _fileSystem, 
                                                    console, 
                                                    projectFileFileFinder, 
                                                    dockerFileFinder, 
                                                    environmentFileFinder);

            versionUpdater.UpgradeToVersion(Path, UseLinuxBaseImage);

            return await Task.FromResult(0);
        }
    }
}