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

        string LinuxRuntimeImageVersion { get; }
        string LinuxPackageRuntimeImageVersion { get; }

        string MonoVersion { get; }
    }
}