namespace Automatize.Version
{
    public class Version2Point1Updater : IDotNetVersionUpdater
    {
        public int MajorVersion => 2;
        public int MinorVersion => 1;
        public string CurrentFramework => "netcoreapp2.0";
        public string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
        public string LinuxSdkImageVersion => "mcr.microsoft.com/dotnet/core/sdk:2.1.505";
        public string AlpineSdkImageVersion => LinuxSdkImageVersion + Alpine.LatestVersion;
        public string LinuxRuntimeImageVersion => "mcr.microsoft.com/dotnet/core/runtime:2.1.9";
        public string AlpineRuntimeImageVersion => LinuxRuntimeImageVersion + Alpine.LatestVersion;
        public string MinimumCommonLibVersion => "1.0.310";
        public string MonoVersion => "5.16.0.179";
    }
}