namespace AutoUpgrade.FileFinders
{
    public class DockerFileFinder : FileFinder
    {
        public DockerFileFinder() : base("DockerFile?*")
        {}
    }
}