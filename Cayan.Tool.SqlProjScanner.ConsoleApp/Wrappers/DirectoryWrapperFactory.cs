namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    public class DirectoryWrapperFactory : IDirectoryWrapperFactory
    {
        public IDirectoryWrapper CreateDirectoryWrapper(string path)
        {
            return new DirectoryWrapper(path);
        }
    }
}
