namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using Wrappers;

    [TestFixture]
    class HtmlReportGeneratorTests
    {
        [Test]
        public void GenerateComparisonReport_ForNoErrors_GeneratesEmptyReport()
        {
            // Setup
            var reportPath = "C:\\Folder1\\Reports\\SqlReportCompare.html";
            var errors = new List<string>();
            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            generator.GenerateComparisonReport(reportPath, errors);

            // Assert
            fileSystemMock.Received(1).WriteAllText(
                "C:\\Folder1\\Reports\\SqlReportCompare.html",
                Arg.Is<string>(x => 
                    x.Contains("No errors found")));
        }

        [Test]
        public void GenerateComparisonReport_ForSimpleErrors_ShowsErrorsInReport()
        {
            // Setup
            var reportPath = "C:\\Folder1\\Reports\\SqlReportCompare.html";
            var errors = new List<string>
            {
                "newDB\\product\\SomeUpdateSp\\@excludeItem|param is defaulted in master but not in new code",
                "newDB\\product\\SomeCreateSp\\@loggingLevel|existing parameter is missing from new code",
                "someDB\\dbo\\SomeDetailsSp\\@breakSomething|new parameter has no default",
                "newDB\\product\\MarkStuffAsUsed\\@removeIcon|existing parameter is out of order",
                "newDB\\product\\MarkStuffAsUsed\\@clearIconPath|existing parameter is out of order"
            };


            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            generator.GenerateComparisonReport(reportPath, errors);

            // Assert
            fileSystemMock.Received(1).WriteAllText(
                "C:\\Folder1\\Reports\\SqlReportCompare.html",
                Arg.Is<string>(x =>
                    !x.Contains("No errors found")
                    && x.Contains("param is defaulted in master but not in new code")
                    && x.Contains("existing parameter is missing from new code")
                    && x.Contains("new parameter has no default")
                    && x.Contains("existing parameter is out of order")
                    ));
        }

        [Test]
        public void GenerateComparisonReport_ForFormattedErrors_ShowsErrorsFormattedInReport()
        {
            // Setup
            var reportPath = "C:\\Folder1\\Reports\\SqlReportCompare.html";
            var errors = new List<string>
            {
                "newDB\\product\\SomeUpdateSp\\@excludeFromMassUpdate|param is defaulted in master but not in new code",
                "newDB\\product\\FtpDetailsLogInsert:FTPDetailsLogInsert\\|sp was renamed with different case"
            };


            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            generator.GenerateComparisonReport(reportPath, errors);

            // Assert
            fileSystemMock.Received(1).WriteAllText(
                "C:\\Folder1\\Reports\\SqlReportCompare.html",
                Arg.Is<string>(x =>
                    !x.Contains("No errors found")
                    && x.Contains("param is defaulted in master but not in new code")
                    && x.Contains("product.F<span>T</span><span>P</span>DetailsLogInsert")
                ));
        }

        [Test]
        public void GenerateComparisonReport_ForNullPath_ThrowsException()
        {
            // Setup
            var errors = new List<string>();
            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => generator.GenerateComparisonReport(null, errors));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter 'reportPath' cannot be null or empty"));
            fileSystemMock.DidNotReceive().AppendAllText(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void GenerateComparisonReport_ForEmptyPath_ThrowsException()
        {
            // Setup
            var errors = new List<string>();
            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => generator.GenerateComparisonReport(string.Empty, errors));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter 'reportPath' cannot be null or empty"));
            fileSystemMock.DidNotReceive().AppendAllText(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void GenerateComparisonReport_ForNullErrors_ThrowsException()
        {
            // Setup
            var reportPath = "C:\\Folder1\\Reports\\ScanReport.csv";
            var fileSystemMock = Substitute.For<IFileWrapper>();

            var generator = new HtmlReportGenerator(fileSystemMock);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => generator.GenerateComparisonReport(reportPath, null));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter 'errors' cannot be null"));
            fileSystemMock.DidNotReceive().AppendAllText(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
