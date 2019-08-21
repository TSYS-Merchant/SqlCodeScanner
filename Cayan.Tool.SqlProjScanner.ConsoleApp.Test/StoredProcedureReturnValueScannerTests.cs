using System.Collections.Generic;

namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NUnit.Framework;
    using System.IO;
    using ReportObjects;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    [TestFixture]
    public class StoredProcedureReturnValueScannerTests
    {
        [Test]
        public void ScanReturnValues_ForExtraNewlinesAndWhiteSpace_Ignores()
        {
            // Setup
            var scanner = new StoredProcedureReturnValueScanner();
            var sqlParser = new TSql120Parser(false);
            var outReport = new StoredProcedureReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
            };

            var sql = SqlSamples.LotsOfWhitespace;
            var parsedSql =
                sqlParser.Parse(new StringReader(sql), out var parseErrors);

            var script = parsedSql as TSqlScript;
            var createSpStatement =
                script?.Batches[0].Statements[0] as CreateProcedureStatement;

            // Act
            scanner.ScanReturnValues(outReport, createSpStatement);

            // Assert
            Assert.That(parseErrors.Count, Is.EqualTo(0));
            Assert.That(outReport.ReturnValues.Count, Is.EqualTo(2));
            Assert.That(outReport.ReturnValues[0].ReturnValueName, Is.EqualTo("'Hello' AS SomeName"));
            Assert.That(outReport.ReturnValues[1].ReturnValueName, Is.EqualTo("'Word555' AS SomeWord"));
        }
    }
}
