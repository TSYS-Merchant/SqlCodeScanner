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
            var spVisitor = new SelectAndDeleteVisitor();
            var spText = ParseSpText(sp);

            sp.Accept(spVisitor);

            foreach (var statement in spVisitor.SelectAndDeleteStatements)
            {
                ParseStatements(spReport, spText, statement);
            }
        }

        private void ParseStatements(StoredProcedureReport spReport,
            string spText, TSqlFragment spStatement)
        {
            switch (spStatement)
            {
                case SelectStatement selectStatement:
                    ParseSelectStatement(selectStatement.QueryExpression, spText, spReport);
                    break;
                case DeleteStatement deleteStatement:
                    ParseDeleteStatement(deleteStatement, spText, spReport);
                    break;
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

        private void ParseSelectStatement(QueryExpression queryExpression,
            string spText, StoredProcedureReport sqlReport)
        {
            var queryVisitor = new QueryExpressionVisitor();
            queryExpression.Accept(queryVisitor);

            foreach (var querySpecification in queryVisitor.QuerySpecifications)
            {
                ParseQueryExpression(querySpecification, spText, sqlReport);
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
