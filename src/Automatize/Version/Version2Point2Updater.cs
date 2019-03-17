namespace Automatize.Version
{
    public class Version2Point2Updater : IDotNetVersionUpdater
    {
        public int MajorVersion => 2;
        public int MinorVersion => 2;
        public string CurrentFramework => "netcoreapp2.1";
        public string MinimumCommonLibVersion => "1.0.345";
        public string LinuxSdkImageVersion => "mcr.microsoft.com/dotnet/core/sdk:2.2.105";
        public string AlpineSdkImageVersion => LinuxSdkImageVersion + Alpine.LatestVersion;
        public string LinuxRuntimeImageVersion => "mcr.microsoft.com/dotnet/core/runtime:2.2.3";
        public string AlpineRuntimeImageVersion => LinuxRuntimeImageVersion + Alpine.LatestVersion;
        public string MonoVersion => "5.16.0.179";

        public string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
    }
}