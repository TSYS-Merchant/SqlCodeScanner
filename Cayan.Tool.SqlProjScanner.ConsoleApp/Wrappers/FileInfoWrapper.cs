namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.IO;

    class FileInfoWrapper : IFileInfoWrapper
    {
        private readonly FileInfo _fileInfo;

        public FileInfoWrapper(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public string FullName => _fileInfo.FullName;
    }
}
