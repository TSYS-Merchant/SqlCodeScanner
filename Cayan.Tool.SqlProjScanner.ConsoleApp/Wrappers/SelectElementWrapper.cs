namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class SelectElementWrapper
    {
        public SelectElement SelectElementHolder { get; set; }

        public int QueryExpressionId { get; set; }

        public string ColumnName { get; set; }

        public bool IsLiteral { get; set; }

        public SelectElementWrapper(SelectElement selectElementHolder,
            int queryExpressionId, string columnName,
            bool isLiteral)
        {
            SelectElementHolder = selectElementHolder;
            QueryExpressionId = queryExpressionId;
            ColumnName = columnName;
            IsLiteral = isLiteral;
        }
    }
}
