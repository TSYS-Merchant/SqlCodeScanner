namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System.Collections.Generic;

    public interface IFileWrapper
    {
        string ReadAllText(string path);

        void WriteAllText(string path, string contents);

        void AppendAllText(string path, string contents);

        bool Exists(string path);

        void Delete(string path);

        void Copy(string sourceFileName, string destFileName);

        IEnumerable<string> ReadLines(string path);
    }
}
