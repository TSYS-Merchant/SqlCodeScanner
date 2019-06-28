namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using System.Collections.Generic;

    public class QueryExpressionVisitor : TSqlFragmentVisitor
    {

        public readonly List<QuerySpecification> QuerySpecifications;

        public QueryExpressionVisitor()
        {
            QuerySpecifications = new List<QuerySpecification>();
        }

        public override void Visit(QuerySpecification node)
        {
            QuerySpecifications.Add(node);
            base.Visit(node);
        }
    }
}
