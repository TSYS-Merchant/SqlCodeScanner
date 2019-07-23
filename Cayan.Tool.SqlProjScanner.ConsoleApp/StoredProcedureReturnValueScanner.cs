namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Text;
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
            var valueName =
                spText.Substring(result.SelectElementHolder.StartOffset,
                    result.SelectElementHolder.FragmentLength);

            valueName =
                valueName
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Replace("\t", "");

            var entry =
                new ReturnSqlReportEntry(valueName, result.QueryExpressionId);

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