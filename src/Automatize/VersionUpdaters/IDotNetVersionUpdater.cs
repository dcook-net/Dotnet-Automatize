namespace Automatize.VersionUpdaters
{
    public interface IDotNetVersionUpdater
    {
        int MajorVersion { get; }
        int MinorVersion { get; }

        string UpdateProjectFileContents(string projectFileContents, bool useLinuxBaseImage);

        string UpdateEnvFileContent(string envFileContents, bool useLinuxBaseImage);

        string UpdateDockerFileContent(string dockerFileContents, bool useLinuxBaseImage);
    }
}