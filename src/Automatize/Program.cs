using System;
using Automatize.FileFinders;
using Automatize.Version;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Automatize
{
    [Command(
        Name = "Upgrade",
        FullName = "Automatically update your .NET projects to the specified version of .NetCore")]
    [Subcommand("Upgrade", typeof(UpgradeCommand))]
    public class Program
    {
        public static int Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton(PhysicalConsole.Singleton)
                .AddSingleton<IProjectFileFinder, ProjectFileFinder>()
                .AddSingleton<IDockerFileFinder, DockerFileFinder>()
                .AddSingleton<IDockerFileFinder, DockerFileFinder>()
                .AddSingleton<IEnvironmentFileFinder, EnvironmentFileFinder>()
                .AddSingleton<IVersionUpdateCollection, VersionUpdaterCollection>()
                .AddSingleton<IUpdaterFacotry, UpdaterFactory>();

            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IDotNetVersionUpdater, Version2Point1Updater>());
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Singleton<IDotNetVersionUpdater, Version2Point2Updater>());

            var services = serviceCollection.BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException cpe)
            {
                Console.WriteLine(cpe.Message);
                return -1;
            }
            finally
            {
#if DEBUG
                Console.ReadLine();
#endif
            }
        }

        protected int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }
    }
}