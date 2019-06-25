namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Linq;
    using Resources;
    using Wrappers;

    public class ConsoleArguments
    {
        public string SolutionPath { get; set; }
        public string DataFilePath { get; set; }
        public string ReportPath { get; set; }
        public bool CreateDataFile { get; set; }

        private readonly IConsoleWrapper _consoleWrapper;

        public ConsoleArguments()
            : this(new ConsoleWrapper())
        {

        }

        public ConsoleArguments(IConsoleWrapper consoleWrapper)
        {
            SolutionPath = null;
            DataFilePath = null;
            ReportPath = null;
            CreateDataFile = false;

            _consoleWrapper = consoleWrapper;
        }

        public bool Parse(string[] args)
        {
            if ((args.Any(arg => arg.ToLower() == "-h" ||
                                arg.ToLower() == "-help"))
                || args.Length == 0)
            {
                _consoleWrapper.WriteLine();
                _consoleWrapper.WriteLine(ConsoleOutput.HelpText);
                return false;
            }

            CreateDataFile =
                args.Any(arg => arg.ToLower() == "create");

            if (!CreateDataFile && args.All(arg => arg.ToLower() != "compare"))
            {
                _consoleWrapper.WriteLine(ConsoleOutput.ArgumentInstruction);
                _consoleWrapper.WriteLine(ConsoleOutput.HelpInstruction);
                return false;
            }

            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-s":
                    case "-solution-path":
                        SolutionPath = ParseNextArgument(args, i);
                        break;
                    case "-data-file":
                    case "-d":
                        DataFilePath = ParseNextArgument(args, i);
                        break;
                    case "-report":
                    case "-r":
                        ReportPath = ParseNextArgument(args, i);
                        break;
                }
            }

            return Validate();
        }

        private bool Validate()
        {
            var argumentError = false;

            if (SolutionPath == null)
            {
                _consoleWrapper.WriteLine(ConsoleOutput.SolutionPathError);
                argumentError = true;
            }

            if (DataFilePath == null)
            {
                _consoleWrapper.WriteLine(ConsoleOutput.DataFileError);
                argumentError = true;
            }
            if (!CreateDataFile && ReportPath == null)
            {
                _consoleWrapper.WriteLine(ConsoleOutput.ReportError);
                argumentError = true;
            }

            return !argumentError;
        }

        private string ParseNextArgument(string[] args, int index)
        {
            return args.Length < index + 2 ? null : args[index + 1];
        }
    }
}
