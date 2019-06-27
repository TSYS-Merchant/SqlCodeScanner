namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Text;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class StoredProcedureReturnValueScanner
    {
        public void ScanReturnValues(SqlReport sqlReport,
            CreateProcedureStatement sp, string db, string schema)
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
                        ParseStatements(sqlReport, sp, db, schema,
                            spText, spStatement);
                    }
                }
                else
                {
                    ParseStatements(sqlReport, sp, db, schema,
                        spText, subStatement);
                }
            }
        }

        private void ParseStatements(SqlReport sqlReport,
            CreateProcedureStatement sp, string db, string schema,
            string spText, TSqlFragment spStatement)
        {
            if (spStatement is SelectStatement selectStatement)
            {
                ParseSelectStatement(selectStatement, spText, sqlReport,
                    db, schema, sp);
            }

            if (spStatement is DeleteStatement deleteStatement)
            {
                ParseDeleteStatement(deleteStatement, spText,
                    sqlReport, db, schema, sp);
            }
        }

        private void ParseDeleteStatement(DeleteStatement deleteStatement,
            string spText, SqlReport sqlReport,
            string db, string schema, CreateProcedureStatement sp)
        {
            if (deleteStatement.DeleteSpecification.OutputClause == null)
            {
                return;
            }
            
            var selectColumns =
                deleteStatement.DeleteSpecification.OutputClause.SelectColumns;

            foreach (var result in selectColumns)
            {
                ParseSelectElement(result, spText, sqlReport,
                    db, schema, sp);
            }
        }

        private void ParseSelectStatement(SelectStatement selectStatement,
            string spText, SqlReport sqlReport,
            string db, string schema, CreateProcedureStatement sp)
        {
            switch (selectStatement.QueryExpression)
            {
                case BinaryQueryExpression binaryQueryExpression:
                {
                    var qe1 = (QuerySpecification)binaryQueryExpression.FirstQueryExpression;
                    var qe2 = (QuerySpecification)binaryQueryExpression.SecondQueryExpression;

                    ParseQueryExpression(qe1, spText,
                        sqlReport, db, schema, sp);

                    ParseQueryExpression(qe2, spText,
                        sqlReport, db, schema, sp);
                    break;
                }

                case QuerySpecification querySpecification:
                    ParseQueryExpression(querySpecification, spText,
                        sqlReport, db, schema, sp);
                    break;
            }
        }

        private void ParseQueryExpression(QuerySpecification qs,
            string spText, SqlReport sqlReport,
            string db, string schema, CreateProcedureStatement sp)
        {
            foreach (var result in qs.SelectElements)
            {
                ParseSelectElement(result, spText, sqlReport,
                    db, schema, sp);
            }
        }

        private void ParseSelectElement(SelectElement result,
            string spText, SqlReport sqlReport,
            string db, string schema, CreateProcedureStatement sp)
        {
            var valueName =
                spText.Substring(result.StartOffset, result.FragmentLength);

            valueName = valueName.Replace("\r", "").Replace("\n", "");

            var spFormattedName =
                sp.ProcedureReference.Name.BaseIdentifier.Value;

            var entry =
                new ReturnSqlReportEntry(db, schema, spFormattedName, valueName);

            sqlReport.ReturnValues.Add(entry);
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
