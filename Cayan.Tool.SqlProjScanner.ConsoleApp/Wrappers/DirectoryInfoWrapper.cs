namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.Collections.Generic;
    using System.IO;

    public class DirectoryInfoWrapper : IDirectoryInfoWrapper
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryInfoWrapper(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        public List<IFileInfoWrapper> GetFiles(string searchPattern)
        {
            var files = _directoryInfo.GetFiles(
                searchPattern, SearchOption.AllDirectories);

            var results = new List<IFileInfoWrapper>();

            foreach (var file in files)
            {
                results.Add(new FileInfoWrapper(file));
            }

            return results;
        }
    }
}
