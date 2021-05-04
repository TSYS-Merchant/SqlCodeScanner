namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Diagnostics;
    using Resources;
    using Wrappers;

    public class SqlReportConsoleManager
    {
        private readonly ISqlFileScanner _fileScanner;
        private readonly IConsoleWrapper _consoleWrapper;

        private const int SuccessReturnValue = 0;
        private const int FailureReturnValue = 2;

        public SqlReportConsoleManager(Configuration config) 
        : this(new SqlFileScanner(config), new ConsoleWrapper())
        {
        }

        internal SqlReportConsoleManager(ISqlFileScanner fileScanner,
            IConsoleWrapper consoleWrapper)
        {
            _fileScanner = fileScanner;
            _consoleWrapper = consoleWrapper;
        }

        public int MakeSqlReport(string[] args)
        {
            var arguments = new ConsoleArguments();
            if (!arguments.Parse(args))
            {
                return FailureReturnValue;
            }

            var timer = new Stopwatch();

            timer.Start();

            var result = _fileScanner.OrchestrateSqlReport
            (arguments.SolutionPath,
                arguments.DataFilePath,
                arguments.ReportPath,
                arguments.CreateDataFile);

            timer.Stop();

            _consoleWrapper.WriteLine($"{ConsoleOutput.TimeTakenMessage} {timer.ElapsedMilliseconds / 1000}s");

            return result == false ? FailureReturnValue : SuccessReturnValue;
        }
    }
}
