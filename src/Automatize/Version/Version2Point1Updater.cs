namespace Automatize.Version
{
    public class Version2Point1Updater : IDotNetVersionUpdater
    {
        public int MajorVersion => 2;
        public int MinorVersion => 1;
        public string CurrentFramework => "netcoreapp2.0";
        public string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
        public string LinuxSdkImageVersion => "mcr.microsoft.com/dotnet/core/sdk:2.1.603";
        public string LinuxRuntimeImageVersion => "mcr.microsoft.com/dotnet/core/aspnet:2.1.10";
        public string LinuxPackageRuntimeImageVersion => "mcr.microsoft.com/dotnet/core/runtime:2.1.603";
        public string MinimumCommonLibVersion => "1.0.310";
        public string MonoVersion => "5.16.0.179";
    }
}