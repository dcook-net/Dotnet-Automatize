namespace AutoUpgrade.VersionUpdaters
{
    public interface IDotNetVersionUpdater
    {
        int MajorVersion { get; }
        int MinorVersion { get; }

        string UpdateProjectFileContents(string projectFileContents);

        string UpdateEnvFileContent(string envFileContents);

        string UpdateDockerFileContent(string dockerFileContents);
    }
}