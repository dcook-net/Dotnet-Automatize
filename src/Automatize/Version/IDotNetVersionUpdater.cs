namespace Automatize.Version
{
    public interface IDotNetVersionUpdater
    {
        int MajorVersion { get; }
        int MinorVersion { get; }
        string CurrentFramework {get;}
        string TargetFramework { get; }

        string MinimumCommonLibVersion { get; }

        string LinuxSdkImageVersion { get; }
        string AlpineSdkImageVersion { get; }

        string LinuxRuntimeImageVersion { get; }
        string AlplineRuntimeImageVersion { get; }

        string MonoVersion { get; }
    }
}