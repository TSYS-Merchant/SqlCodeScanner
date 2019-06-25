namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Text;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class StoredProcedureReturnValueScanner
    {
        public void ScanReturnValues(SqlReport sqlReport,
            CreateProcedureStatement sp, string db, string schema,
            SqlReportWriter sqlReturnValueReportWriter)
        {

            // All statements in the .sql file
            foreach (var subStatement in sp.StatementList.Statements)
            {
                // The create SP statement is always a block statement
                if (!(subStatement is BeginEndBlockStatement blockStatement))
                {
                    continue;
                }

                var spText = ParseSpText(blockStatement);

                // Individual parts of the SP, e.g. SELECT and INSERT INTO
                foreach (var spStatement in blockStatement.StatementList.Statements)
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
            if (!(selectStatement.QueryExpression is QuerySpecification))
            {
                return;
            }

            var queryExpression = 
                (QuerySpecification)selectStatement.QueryExpression;

            foreach (var result in queryExpression.SelectElements)
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

        private string ParseSpText(BeginEndBlockStatement blockStatement)
        {
            var queryTextSb = new StringBuilder();

            foreach (var element in blockStatement.ScriptTokenStream)
            {
                queryTextSb.Append(element.Text);
            }

            return queryTextSb.ToString();
        }
}
}
