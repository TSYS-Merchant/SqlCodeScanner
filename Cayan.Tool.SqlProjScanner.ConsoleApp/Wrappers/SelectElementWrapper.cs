namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class SelectElementWrapper
    {
        public SelectElement SelectElementHolder { get; set; }

        public int QueryExpressionId { get; set; }

        public SelectElementWrapper(SelectElement selectElementHolder,
            int queryExpressionId)
        {
            SelectElementHolder = selectElementHolder;
            QueryExpressionId = queryExpressionId;
        }
    }
}
