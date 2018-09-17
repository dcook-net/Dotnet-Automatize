using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace AutoUpgrade.FileFinders
{
    public abstract class FileFinder
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _searchPattern;

        protected FileFinder(IFileSystem fileSystem, string searchPattern)
        {
            _fileSystem = fileSystem;
            _searchPattern = searchPattern;
        }

        public IEnumerable<FileInfo> Search(string directoryToSearch)
        {
            var fileNames = _fileSystem.Directory.GetFiles(directoryToSearch, _searchPattern, SearchOption.AllDirectories);
            return fileNames.Select(fileName => new FileInfo(fileName)).ToList();
        }
    }
}