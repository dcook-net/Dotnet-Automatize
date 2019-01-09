namespace Automatize.Version
{
    public class Version2Point2Updater : IDotNetVersionUpdater
    {
        public int MajorVersion => 2;
        public int MinorVersion => 2;
        public string CurrentFramework => "netcoreapp2.1";
        public string MinimumCommonLibVersion => "1.0.345";
        public string LinuxSdkImageVersion => "dotnet:2.2.102-sdk";
        public string AlpineSdkImageVersion => "dotnet:2.2.102-sdk-alpine3.8";
        public string LinuxRuntimeImageVersion => "dotnet:2.2.1-aspnetcore-runtime";
        public string AlplineRuntimeImageVersion => "dotnet:2.2.1-aspnetcore-runtime-alpine3.8";
        public string MonoVersion => "5.16.0.179";

        public string TargetFramework => $"netcoreapp{MajorVersion}.{MinorVersion}";
    }
}