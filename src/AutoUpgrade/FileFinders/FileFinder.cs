using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoUpgrade.FileFinders
{
    public abstract class FileFinder
    {
        private readonly string _searchPattern;

        protected FileFinder(string searchPattern)
        {
            _searchPattern = searchPattern;
        }

        public IEnumerable<FileInfo> Search(string directoryToSearch)
        {
            var fileNames = Directory.GetFiles(directoryToSearch, _searchPattern, SearchOption.AllDirectories);
            return fileNames.Select(fileName => new FileInfo(fileName)).ToList();
        }
    }
}