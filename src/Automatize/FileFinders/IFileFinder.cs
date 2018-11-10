using System.Collections.Generic;
using System.IO;

namespace Automatize.FileFinders
{
    public interface IFileFinder
    {
        IEnumerable<FileInfo> Search(string directoryToSearch);
    }
}