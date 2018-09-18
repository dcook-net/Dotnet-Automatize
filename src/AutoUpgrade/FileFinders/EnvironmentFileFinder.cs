namespace AutoUpgrade.FileFinders
{
    public class EnvironmentFileFinder : FileFinder
    {
        public EnvironmentFileFinder() : base("*.env")
        { }
    }
}