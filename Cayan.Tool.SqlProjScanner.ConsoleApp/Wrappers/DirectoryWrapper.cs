namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.Collections.Generic;
    using System.IO;

    public class DirectoryWrapper : IDirectoryWrapper
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryWrapper(string path)
        {
            _directoryInfo = new DirectoryInfo(path);
        }

        public List<IDirectoryInfoWrapper> GetDirectories(string searchPattern)
        {
            var directoryInfo = _directoryInfo.GetDirectories(
                searchPattern, SearchOption.AllDirectories);

            var results = new List<IDirectoryInfoWrapper>();

            foreach (var infoItem in directoryInfo)
            {
                results.Add(new DirectoryInfoWrapper(infoItem));
            }

            return results;
        }
    }
}
