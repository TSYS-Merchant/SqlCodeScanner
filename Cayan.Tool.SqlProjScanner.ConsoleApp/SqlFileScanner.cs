namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using ReportObjects;
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Wrappers;

    public class SqlFileScanner : ISqlFileScanner
    {
        private readonly SqlReportWriter _sqlParameterReportWriter;
        private readonly SqlReport _sqlReport;

        private readonly IFileWrapper _fileWrapper;
        private readonly IDirectoryWrapperFactory _directoryFactory;
        private readonly IHtmlReportGenerator _htmlReportGenerator;
        private readonly IParamReportComparer _paramReportComparer;
        private readonly IReturnReportComparer _returnReportComparer;

        private readonly string _storedProcedureDirectoryName;

        private readonly string _schemaIgnoreListFileName;

        public SqlFileScanner(Configuration config)
            :this(new FileWrapper(), new XmlStreamWrapperFactory(),
                new DirectoryWrapperFactory(), new HtmlReportGenerator(),
                new ParamReportComparer(), new ReturnReportComparer(),
                config)
        {
        }

        internal SqlFileScanner(IFileWrapper fileWrapper,
            IXmlStreamWrapperFactory xmlWriterWrapperFactory,
            IDirectoryWrapperFactory directoryWrapperFactory,
            IHtmlReportGenerator htmlReportGenerator,
            IParamReportComparer paramReportComparer,
            IReturnReportComparer returnReportComparer,
            Configuration config)
        {
            _fileWrapper = fileWrapper;

            _sqlParameterReportWriter =
                new SqlReportWriter(xmlWriterWrapperFactory, _fileWrapper);

            _directoryFactory = directoryWrapperFactory;
            _htmlReportGenerator = htmlReportGenerator;
            _paramReportComparer = paramReportComparer;
            _returnReportComparer = returnReportComparer;

            _sqlReport = new SqlReport();

            _storedProcedureDirectoryName = config.AppSettings.StoredProcedureDirectoryName;
            _schemaIgnoreListFileName = config.AppSettings.SchemaIgnoreListFileName;
        }

        public bool OrchestrateSqlReport(string sqlCodePath, string dataFilePath,
            string htmlReportPath, bool createDataFile)
        {
            if (string.IsNullOrEmpty(sqlCodePath))
            {
                throw new ArgumentException("solution-path cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(dataFilePath))
            {
                throw new ArgumentException("data-file path cannot be null or empty.");
            }

            var ignoreFilePath = $"{sqlCodePath}\\{_schemaIgnoreListFileName}";
            var ignoreList = new IgnoreList(_fileWrapper, ignoreFilePath);

            if (!createDataFile && string.IsNullOrEmpty(htmlReportPath))
            {
                throw new ArgumentException("report path cannot be null or empty.");
            }

            ScanSqlFilesInPath(sqlCodePath, ignoreList);

            if (createDataFile)
            {
                _sqlParameterReportWriter.WriteMasterReport(dataFilePath, _sqlReport);

                return true;
            }

            var errors = new List<string>();

            CompareSqlReports(errors, dataFilePath, htmlReportPath);

            return errors.Count == 0;
        }

        private void CompareSqlReports(List<string> errors, string dataFilePath,
            string htmlReportPath)
        {
            var masterReport =
                _sqlParameterReportWriter.LoadMasterReport(dataFilePath);

            _paramReportComparer.CompareReports(masterReport, _sqlReport, errors);
            _returnReportComparer.CompareReports(masterReport, _sqlReport, errors);

            // Generate HTML Report
            _htmlReportGenerator.GenerateComparisonReport(htmlReportPath, errors);
        }

        private void ScanSqlFilesInPath(string sqlCodePath,
            IgnoreList ignoreList)
        {
            var sqlDirectory = _directoryFactory.CreateDirectoryWrapper(sqlCodePath);

            var spDirectories = sqlDirectory.GetDirectories($"*{_storedProcedureDirectoryName}*");

            foreach (var spDirectory in spDirectories)
            {
                var allSps = spDirectory.GetFiles("*.sql");

                if (allSps == null)
                {
                    continue;
                }

                foreach (var sp in allSps)
                {
                    var tSqlScript = ParseSp(sp);
                    ScanSp(tSqlScript, sp.FullName, ignoreList);
                }
            }
        }

        private TSqlScript ParseSp(IFileInfoWrapper spFile)
        {
            var sql = _fileWrapper.ReadAllText(spFile.FullName);
            
            var sqlParser = new TSql140Parser(false);

            var parsedSql =
                sqlParser.Parse(new StringReader(sql), out var parseErrors);

            if (parseErrors.Count != 0)
            {
                throw new Exception($"Error parsing SQL file {spFile.FullName}");
            }

            var nonCommentTokens =
                parsedSql.ScriptTokenStream
                    .Where(x => x.TokenType != TSqlTokenType.MultilineComment)
                    .Where(x => x.TokenType != TSqlTokenType.SingleLineComment)
                    .Select(x => x.Text);

            var result =
                sqlParser.Parse(new StringReader(string.Join("", nonCommentTokens)), out var parseErrors2);

            if (parseErrors2.Count != 0)
            {
                throw new Exception($"Error parsing SQL with comments removed {spFile.FullName}");
            }

            return result as TSqlScript;
        }

        private void ScanSp(TSqlScript tSqlScript, string fullPath,
            IgnoreList ignoreList)
        {
            var parameterScanner = new StoredProcedureParameterScanner();
            var returnValueScanner = new StoredProcedureReturnValueScanner();

            var solutionSchemaOffset = 3;
            var dbOffset = 4;
            var scriptParts = fullPath.Split('\\');

            var schema = scriptParts[scriptParts.Length - solutionSchemaOffset];
            var db = scriptParts[scriptParts.Length - dbOffset];

            if (ignoreList.IsSchemaIgnored(db, schema))
            {
                return;
            }

            foreach (var batch in tSqlScript.Batches)
            {
                foreach (var statement in batch.Statements)
                {
                    if (!(statement is CreateProcedureStatement sp))
                    {
                        continue;
                    }

                    var spFormattedName =
                        sp.ProcedureReference.Name.BaseIdentifier.Value;

                    var spReport =
                        new StoredProcedureReport(db, schema, spFormattedName);

                    parameterScanner.ScanSpParameters(spReport, sp);

                    returnValueScanner.ScanReturnValues(spReport, sp);

                    _sqlReport.StoredProcedures.Add(spReport);
                }
            }
        }

    }
}
