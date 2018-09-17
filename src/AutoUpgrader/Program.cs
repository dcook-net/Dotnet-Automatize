using System;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace AutoUpgrader
{
    [Command(
        Name = "AutoUpgrader",
        FullName = "Automagically update your .NET projects to the specified version of .NetCore")]
//    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
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
                Console.ReadLine();
            }
        }

        // ReSharper disable once UnusedMember.Global
        protected int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion() => typeof(Program)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
    }


//    public class Settings
//    {
//        public string PathToSolution { get; set; }
//        public string TargetFramework { get; set; } = "netcoreapp2.1";
//        public string SDKDockerImage { get; set; } = "microsoft/dotnet:2.1-sdk";
//        public string RuntimeDockerImage { get; set; } = "microsoft/dotnet:2.1-aspnetcore-runtime";
//        public string SDKVersion { get; set; } = "15.8.0";
//    }

    //this is running as a global tool, so it can execute in any location
    //therefore i need
    //fully qualified path to directory of solution to update : "c:\dev\energy.enquiry-presenter\"
    //the version of .Net we are upgrading to : TargetFramework : "netcoreapp2.1" 
    //the docker image to use
    // SDK for building : "microsoft/dotnet:2.1-sdk"
    // runtime : "microsoft/aspnetcore:2.1"
    //test skd version "15.8.0"

    //Validate args - nothing is empty
    //if missing, use defaults or error?
    

    //optional extras
    //git add -A
    //git commit -m "update message"
    //git checkout -b "branchName"
    //git push origin -u "branchName"
}