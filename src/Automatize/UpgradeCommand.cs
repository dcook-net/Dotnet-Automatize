using System.IO;
using System.Threading.Tasks;
using Automatize.Version;
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
        private readonly IVersionUpdateCollection _versionUpdaterCollection;
        private readonly IUpdaterFacotry _updaterFactory;

        [Argument(0, ShowInHelpText = true, Name = "path", Description = "Path to the directory to upgrade. Defaults to current location.")]
        [DirectoryExists]
        public string Path { get; private set; }

        [Option(CommandOptionType.NoValue, ShowInHelpText = true, ShortName = "l", LongName = "useLinux",
            Description = "The Base image to use in your DockerFile. Valid options are 'Alpine' or 'Linux'")]
        public bool UseLinuxBaseImage { get; } = false;

        [Option(CommandOptionType.SingleOrNoValue, ShowInHelpText = true, ShortName="mv", LongName = "minorversion", Description = "Specify which minor version of .Net Core you want 2 or 1. Major version is always 2 at the moment.")]
        public int MinorVersion { get; set; } = 2;

        private static int MajorVersion => 2;

        public UpgradeCommand(IVersionUpdateCollection versionUpdaterCollection, IUpdaterFacotry updaterFactory)
        {
            _versionUpdaterCollection = versionUpdaterCollection;
            _updaterFactory = updaterFactory;
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

            console.WriteLine($"Starting updating of {Path} to .Net Core v{MajorVersion}.{MinorVersion}");
            
            var dotNetVersionUpdater = _versionUpdaterCollection.Find(MajorVersion, MinorVersion);

            var updater = _updaterFactory.Build(dotNetVersionUpdater);

            updater.UpgradeToVersion(Path, UseLinuxBaseImage);

            return await Task.FromResult(0);
        }
    }
}