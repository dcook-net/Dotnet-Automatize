using Automatize.Version;

namespace Automatize
{
    public interface IUpdaterFacotry
    {
        Updater Build(IDotNetVersionUpdater dotNetVersionUpdater);
    }
}