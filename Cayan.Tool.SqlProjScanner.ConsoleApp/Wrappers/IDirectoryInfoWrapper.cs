namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.Collections.Generic;

    public interface IDirectoryInfoWrapper
    {
        List<IFileInfoWrapper> GetFiles(string searchPattern);
    }
}
