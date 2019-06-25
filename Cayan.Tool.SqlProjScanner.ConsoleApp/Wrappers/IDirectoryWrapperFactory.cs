namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    public interface IDirectoryWrapperFactory
    {
        IDirectoryWrapper CreateDirectoryWrapper(string path);
    }
}
