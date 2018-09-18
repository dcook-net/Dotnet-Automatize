using System.IO;
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
        [Argument(0, Name = "path", Description = "Path to the directory to upgrade")]
        [DirectoryExists]
        public string Path { get; private set; }

//        private readonly IFileSystem _fileSystem;
//
//        public UpgradeCommand() : this(null)
//        {}
//
//        public UpgradeCommand(IFileSystem fileSystem)
//        {
//            _fileSystem = fileSystem ?? new FileSystem();
//        }

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

//            if (!_fileSystem.GetAttributes(Path).HasFlag(FileAttributes.Directory))
//            {
//                console.WriteLine("Supplied Path is not a Directory. Aborting");
//                return await Task.FromResult(-1);
//            }

            //var directory = TargetFolder(settings.PathToSolution);
            console.WriteLine($"Starting updating of {Path}");

            var projectFileFileFinder = new ProjectFileFinder();
            var dockerFileFinder = new DockerFileFinder();
            var environmentFileFinder = new EnvironmentFileFinder();
            var fileUpdater = new FileUpdater(console);

            var dockerFiles = dockerFileFinder.Search(Path);
            var projectFiles = projectFileFileFinder.Search(Path);
            var envFiles = environmentFileFinder.Search(Path);

            //TODO: inject the version updater based on the version the user wants
            fileUpdater.UpdateProjectFiles(projectFiles, new Version2Point1Updater());
            fileUpdater.UpdateDockerFiles(dockerFiles, new Version2Point1Updater());
            fileUpdater.UpdateEnvironmentFiles(envFiles, new Version2Point1Updater());

            return await Task.FromResult(0);
        }
//
//        private static IFolder TargetFolder(string solutionFolder)
//        {
//            if (string.IsNullOrWhiteSpace(solutionFolder))
//            {
//                solutionFolder = Directory.GetCurrentDirectory();
//            }
//
//            if (!Directory.Exists(solutionFolder))
//            {
//                Console.WriteLine($"Path does not exist: {solutionFolder}");
//                return null;
//            }
//            
////            Directory.SetCurrentDirectory(solutionFolder);
//
//            return new Folder(new DirectoryInfo(solutionFolder));
//        }
    }
}