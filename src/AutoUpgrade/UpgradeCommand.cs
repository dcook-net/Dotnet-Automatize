using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using AutoUpgrade.FileFinders;
using AutoUpgrade.VersionUpdaters;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpgrade
{
    [Command(
        Description = "Searches the specified directory and upgrades various files to the requested version of .Net",
        Name = "dotnet autoupgrade",
        FullName = "dotnet-autoupgrade")]
    [HelpOption]
    public class UpgradeCommand
    {
        [Argument(0, ShowInHelpText = true, Name = "path", Description = "Path to the directory to upgrade. Defaults to current location.")]
        [DirectoryExists]
        public string Path { get; private set; }

        [Argument(1, ShowInHelpText = true, Description = "bool flag indicating whether the DOTNET TEST command should run when Upgrage is complete. Defaults to false.", Name = "Test")]
        public bool Test { get; set; } = false;

        private readonly IFileSystem _fileSystem;

        public UpgradeCommand() : this(null)
        {

        }

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
            var fileUpdater = new FileUpdater(console);

            var dockerFiles = dockerFileFinder.Search(Path);
            var projectFiles = projectFileFileFinder.Search(Path);
            var envFiles = environmentFileFinder.Search(Path);

            //TODO: inject the version updater based on the version the user wants
            var dotNetVersionUpdater = new Version2Point1Updater();

            fileUpdater.UpdateProjectFiles(projectFiles, dotNetVersionUpdater);
            fileUpdater.UpdateDockerFiles(dockerFiles, dotNetVersionUpdater);
            fileUpdater.UpdateEnvironmentFiles(envFiles, dotNetVersionUpdater);

            if (Test)
            {
                var dotnet = new DotNet();
                dotnet.Test();
            }

            return await Task.FromResult(0);
        }
    }
}