namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System;

    public class ConsoleWrapper : IConsoleWrapper
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
