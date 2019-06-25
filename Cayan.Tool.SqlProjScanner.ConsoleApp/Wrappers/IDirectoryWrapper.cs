namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.Collections.Generic;

    public interface IDirectoryWrapper
    {
        List<IDirectoryInfoWrapper> GetDirectories(string searchPattern);
    }
}
