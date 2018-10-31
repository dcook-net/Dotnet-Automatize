using System;
using McMaster.Extensions.CommandLineUtils;

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
            var app = new CommandLineApplication<Program> {ThrowOnUnexpectedArgument = false};
            app.Conventions.UseDefaultConventions().UseConstructorInjection();

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