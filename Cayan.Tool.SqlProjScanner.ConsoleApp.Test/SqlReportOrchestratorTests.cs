namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using Wrappers;

    [TestFixture]
    public class SqlReportOrchestratorTests
    {
        [Test]
        public void MakeSqlReport_ForSuccess_ReturnsZero()
        {
            // Setup
            var scanner = Substitute.For<ISqlFileScanner>();
            var console = Substitute.For<IConsoleWrapper>();

            scanner.OrchestrateSqlReport(Arg.Any<string>(),
                    Arg.Any<string>(), Arg.Any<string>(),
                    Arg.Any<bool>())
                .Returns(true);
            
            var orchestrator = new SqlReportConsoleManager(scanner, console);

            string[] args =
            {
                "compare",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

                // Act
            var result = 
                orchestrator.MakeSqlReport(args);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void MakeSqlReport_ForFailure_ReturnsTwo()
        {
            // Setup
            var scanner = Substitute.For<ISqlFileScanner>();
            var console = Substitute.For<IConsoleWrapper>();

            scanner.OrchestrateSqlReport(Arg.Any<string>(),
                     Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(false);

            var orchestrator = new SqlReportConsoleManager(scanner, console);

            string[] args =
            {
                "compare",
                "-solution-path",
                "folder1\\projFile",
                "-data-file",
                "reports\\master.xml",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result =
                orchestrator.MakeSqlReport(args);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void MakeSqlReport_TooFewParameters_ReturnsTwo()
        {
            // Setup
            var scanner = Substitute.For<ISqlFileScanner>();
            var console = Substitute.For<IConsoleWrapper>();

            scanner.OrchestrateSqlReport(Arg.Any<string>(),
                     Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(true);

            var orchestrator = new SqlReportConsoleManager(scanner, console);

            string[] args =
            {
                "compare",
                "-solution-path",
                "folder1\\projFile",
                "-report",
                "reports\\scanReport.html"
            };

            // Act
            var result =
                orchestrator.MakeSqlReport(args);

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }
    }
}
