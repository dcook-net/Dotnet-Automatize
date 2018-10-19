namespace AutoUpgrade.VersionUpdaters
{
    public interface IDotNetVersionUpdater
    {
        int MajorVersion { get; }
        int MinorVersion { get; }

        string UpdateProjectFileContents(string projectFileContents, string baseImage);

        string UpdateEnvFileContent(string envFileContents, string baseImage);

        string UpdateDockerFileContent(string dockerFileContents, string baseImage);
    }
}