namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using Wrappers;

    public class StoredProcedureReturnValueScanner
    {
        public void ScanReturnValues(StoredProcedureReport spReport,
            CreateProcedureStatement sp)
        {
            var returnValueVisitor = new SelectElementVisitor();
            var spText = ParseSpText(sp);

            sp.Accept(returnValueVisitor);

            foreach (var returnValue in returnValueVisitor.ReturnValues)
            {
                ParseSelectElement(returnValue, spText, spReport);
            }
        }

        private void ParseSelectElement(SelectElementWrapper result,
            string spText, StoredProcedureReport spReport)
        {
            var isLiteral = CheckIfLiteral(result);

            var valueName =
                spText.Substring(result.SelectElementHolder.StartOffset,
                    result.SelectElementHolder.FragmentLength);

            valueName =
                valueName
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ");

            valueName =
                Regex.Replace(valueName, @"\s+", " ");

            var entry =
                new ReturnSqlReportEntry(valueName, result.QueryExpressionId,
                    isLiteral);

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

        private bool CheckIfLiteral(SelectElementWrapper result)
        {
            if (!(result.SelectElementHolder is SelectScalarExpression scalarExpression))
            {
                return false;
            }

            return scalarExpression.Expression is Literal;
        }
    }
}