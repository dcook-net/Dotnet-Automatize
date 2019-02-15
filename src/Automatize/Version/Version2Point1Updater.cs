namespace Automatize.Version
{
    public class Version2Point1Updater : IDotNetVersionUpdater
    {
        public int MajorVersion => 2;
        public int MinorVersion => 1;
        public string CurrentFramework => "netcoreapp2.0";
        public string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
        public string LinuxSdkImageVersion => "dotnet:2.1.500-sdk";
        public string AlpineSdkImageVersion => "dotnet:2.1.500-sdk-alpine3.7";
        public string LinuxRuntimeImageVersion => "dotnet:2.1.6-aspnetcore-runtime";
        public string AlpineRuntimeImageVersion => "dotnet:2.1.6-aspnetcore-runtime-alpine3.7";
        public string MinimumCommonLibVersion => "1.0.310";
        public string MonoVersion => "5.16.0.179";
    }
}