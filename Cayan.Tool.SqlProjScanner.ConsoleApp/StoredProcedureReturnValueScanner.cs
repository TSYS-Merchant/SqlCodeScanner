namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Text;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class StoredProcedureReturnValueScanner
    {
        public void ScanReturnValues(StoredProcedureReport spReport,
            CreateProcedureStatement sp)
        {

            // All statements in the .sql file
            foreach (var subStatement in sp.StatementList.Statements)
            {
                var spText = ParseSpText(subStatement);

                // The create SP statement is always a block statement
                if (subStatement is BeginEndBlockStatement blockStatement)
                {
                    // Individual parts of the SP, e.g. SELECT and INSERT INTO
                    foreach (var spStatement in blockStatement.StatementList.Statements)
                    {
                        ParseStatements(spReport, spText, spStatement);
                    }
                }
                else
                {
                    ParseStatements(spReport, spText, subStatement);
                }
            }
        }

        private void ParseStatements(StoredProcedureReport spReport,
            string spText, TSqlFragment spStatement)
        {
            if (spStatement is SelectStatement selectStatement)
            {
                ParseSelectStatement(selectStatement, spText, spReport);
            }

            if (spStatement is DeleteStatement deleteStatement)
            {
                ParseDeleteStatement(deleteStatement, spText, spReport);
            }
        }

        private void ParseDeleteStatement(DeleteStatement deleteStatement,
            string spText, StoredProcedureReport spReport)
        {
            if (deleteStatement.DeleteSpecification.OutputClause == null)
            {
                return;
            }
            
            var selectColumns =
                deleteStatement.DeleteSpecification.OutputClause.SelectColumns;

            foreach (var result in selectColumns)
            {
                ParseSelectElement(result, spText, spReport);
            }
        }

        private void ParseSelectStatement(SelectStatement selectStatement,
            string spText, StoredProcedureReport sqlReport)
        {
            switch (selectStatement.QueryExpression)
            {
                case BinaryQueryExpression binaryQueryExpression:
                {
                    var qe1 = (QuerySpecification)binaryQueryExpression.FirstQueryExpression;
                    var qe2 = (QuerySpecification)binaryQueryExpression.SecondQueryExpression;

                    ParseQueryExpression(qe1, spText,
                        sqlReport);

                    ParseQueryExpression(qe2, spText,
                        sqlReport);
                    break;
                }

                case QuerySpecification querySpecification:
                    ParseQueryExpression(querySpecification, spText,
                        sqlReport);
                    break;
            }
        }

        private void ParseQueryExpression(QuerySpecification qs,
            string spText, StoredProcedureReport spReport)
        {
            foreach (var result in qs.SelectElements)
            {
                ParseSelectElement(result, spText, spReport);
            }
        }

        private void ParseSelectElement(SelectElement result,
            string spText, StoredProcedureReport spReport)
        {
            var valueName =
                spText.Substring(result.StartOffset, result.FragmentLength);

            valueName = valueName.Replace("\r", "").Replace("\n", "");

            var entry =
                new ReturnSqlReportEntry(valueName);

            spReport.ReturnValues.Add(entry);
        }

        private string ParseSpText(TSqlStatement statement)
        {
            var queryTextSb = new StringBuilder();

            foreach (var element in statement.ScriptTokenStream)
            {
                queryTextSb.Append(element.Text);
            }

            return queryTextSb.ToString();
        }
    }
}
