namespace Automatize.Version
{
    public interface IVersionUpdateCollection
    {
        IDotNetVersionUpdater GetLatest();
        IDotNetVersionUpdater Find(int major, int minor);
    }
}