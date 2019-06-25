namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Wrappers;

    [TestFixture]
    public class ConsoleArgumentsTests
    {
        [Test]
        public void Parse_ForNoArguments_ReturnsHelpMessage()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.HelpText)));
        }

        [Test]
        public void Parse_ForDashH_ReturnsHelpMessage()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "-h"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.HelpText)));
        }

        [Test]
        public void Parse_ForHelp_ReturnsHelpMessage()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "-help"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.HelpText)));
        }

        [Test]
        public void Parse_ForHelpVaryingCase_ReturnsHelpMessage()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "-HELP"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.HelpText)));
        }

        [Test]
        public void Parse_ForValidCreate_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "create",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo(null));
        }

        [Test]
        public void Parse_ForValidCreateWithDifferentCase_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "Create",
                "-Solution-Path",
                "folder1\\projFile",
                "-Data-file",
                "reports\\master.xml"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo(null));
        }

        [Test]
        public void Parse_ForValidCreateWithHtmlReport_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "create",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.html"));
        }

        [Test]
        public void Parse_ForCreateWithAlternateExtension_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "create",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.txt"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.txt"));
            Assert.That(parser.ReportPath, Is.EqualTo(null));
        }

        [Test]
        public void Parse_ForValidCompare_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "Compare",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.html"));
        }

        [Test]
        public void Parse_CompareWithAlternateExtensions_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "Compare",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.txt",
                "-report",
                "reports\\scanReport.txt"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.txt"));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.txt"));
        }

        [Test]
        public void Parse_ForValidCompareWithDifferingCase_ParsesArguments()
        {
            // Setup
            var parser = new ConsoleArguments();

            string[] args =
            {
                "compare",
                "-Solution-path",
                "folder1\\projFile",
                "-Data-File",
                "reports\\master.xml",
                "-Report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.html"));
        }

        [Test]
        public void Parse_ForInvalidCommand_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "wrong",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo(null));
            Assert.That(parser.DataFilePath, Is.EqualTo(null));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.ArgumentInstruction)));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.HelpInstruction)));
        }

        [Test]
        public void Parse_ForCreateMissingSolutionPath_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "create",
                "-data-file",
                "reports\\master.xml"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo(null));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.SolutionPathError)));
        }

        [Test]
        public void Parse_ForCreateMissingDataFile_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "create",
                "-solution-path",
                "folder1\\projFile"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo(null));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.DataFileError)));
        }

        [Test]
        public void Parse_ForCreateMissingAllArguments_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "create"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(true));
            Assert.That(parser.SolutionPath, Is.EqualTo(null));
            Assert.That(parser.DataFilePath, Is.EqualTo(null));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.DataFileError)));
        }

        [Test]
        public void Parse_CompareMissingSolutionPath_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "compare",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo(null));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.html"));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.SolutionPathError)));
        }

        [Test]
        public void Parse_CompareMissingDataFile_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "compare",
                "-solution-path",
                "folder1\\projFile",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo(null));
            Assert.That(parser.ReportPath, Is.EqualTo("reports\\scanReport.html"));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.DataFileError)));
        }

        [Test]
        public void Parse_CompareMissingReport_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "compare",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo("folder1\\projFile"));
            Assert.That(parser.DataFilePath, Is.EqualTo("reports\\master.xml"));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.ReportError)));
        }

        [Test]
        public void Parse_CompareMissingAll_DoesNotParse()
        {
            // Setup
            var console = Substitute.For<IConsoleWrapper>();
            var parser = new ConsoleArguments(console);

            string[] args =
            {
                "compare"
            };

            // Act
            var result = parser.Parse(args);

            // Assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(parser.CreateDataFile, Is.EqualTo(false));
            Assert.That(parser.SolutionPath, Is.EqualTo(null));
            Assert.That(parser.DataFilePath, Is.EqualTo(null));
            Assert.That(parser.ReportPath, Is.EqualTo(null));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.SolutionPathError)));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.DataFileError)));

            console.Received(1).WriteLine(Arg.Is<string>(
                x => x.Contains(ConsoleOutput.ReportError)));
        }
    }
}
