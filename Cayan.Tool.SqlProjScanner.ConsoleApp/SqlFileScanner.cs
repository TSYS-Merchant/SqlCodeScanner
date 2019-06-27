namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using ReportObjects;
    using System;
    using System.Configuration;
    using System.IO;
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

        private readonly string _storedProcedureDirectoryName =
            ConfigurationManager.AppSettings["StoredProcedureDirectoryName"];

        public SqlFileScanner()
            :this(new FileWrapper(), new XmlStreamWrapperFactory(),
                new DirectoryWrapperFactory(), new HtmlReportGenerator(),
                new ParamReportComparer(), new ReturnReportComparer())
        {
        }

        internal SqlFileScanner(IFileWrapper fileWrapper,
            IXmlStreamWrapperFactory xmlWriterWrapperFactory,
            IDirectoryWrapperFactory directoryWrapperFactory,
            IHtmlReportGenerator htmlReportGenerator,
            IParamReportComparer paramReportComparer,
            IReturnReportComparer returnReportComparer)
        {
            _fileWrapper = fileWrapper;

            _sqlParameterReportWriter =
                new SqlReportWriter(xmlWriterWrapperFactory, _fileWrapper);

            _directoryFactory = directoryWrapperFactory;
            _htmlReportGenerator = htmlReportGenerator;
            _paramReportComparer = paramReportComparer;
            _returnReportComparer = returnReportComparer;

            _sqlReport = new SqlReport();
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

            if (!createDataFile && string.IsNullOrEmpty(htmlReportPath))
            {
                throw new ArgumentException("report path cannot be null or empty.");
            }

            ScanSqlFilesInPath(sqlCodePath);

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

        private void ScanSqlFilesInPath(string sqlCodePath)
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
                    ScanSp(tSqlScript, sp.FullName);
                }
            }
        }

        private TSqlScript ParseSp(IFileInfoWrapper spFile)
        {
            var sql = _fileWrapper.ReadAllText(spFile.FullName);
            
            var sqlParser = new TSql120Parser(false);

            var result =
                sqlParser.Parse(new StringReader(sql), out var parseErrors);

            if (parseErrors.Count != 0)
            {
                throw new Exception($"Error parsing SQL file {spFile.FullName}");
            }

            return result as TSqlScript;
        }

        private void ScanSp(TSqlScript tSqlScript, string fullPath)
        {
            var parameterScanner = new StoredProcedureParameterScanner();
            var returnValueScanner = new StoredProcedureReturnValueScanner();

            var solutionSchemaOffset = 3;
            var dbOffset = 4;
            var scriptParts = fullPath.Split('\\');

            var schema = scriptParts[scriptParts.Length - solutionSchemaOffset];
            var db = scriptParts[scriptParts.Length - dbOffset];

            foreach (var batch in tSqlScript.Batches)
            {
                foreach (var statement in batch.Statements)
                {
                    if (!(statement is CreateProcedureStatement sp))
                    {
                        continue;
                    }

                    parameterScanner.ScanSpParameters(
                        _sqlReport, sp, db, schema);

                    returnValueScanner.ScanReturnValues(
                        _sqlReport, sp, db, schema);
                }
            }
        }

    }
}
