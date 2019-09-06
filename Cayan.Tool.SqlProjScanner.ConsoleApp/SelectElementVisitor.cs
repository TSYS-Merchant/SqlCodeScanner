namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using Wrappers;

    public class SelectElementVisitor : TSqlFragmentVisitor
    {
        public readonly List<SelectElementWrapper> ReturnValues;
        private int _queryExpressionId;

        public SelectElementVisitor()
        {
            ReturnValues = new List<SelectElementWrapper>();
            _queryExpressionId = 0;
        }

        public override void Visit(QueryExpression node)
        {
            _queryExpressionId += 1;
            base.Visit(node);
        }

        public override void Visit(SelectScalarExpression node)
        {
            var columnName = node.ColumnName is null ? string.Empty : node.ColumnName.Value;
            var isLiteral = node.Expression is Literal;

            ReturnValues.Add(new SelectElementWrapper(node, _queryExpressionId, columnName, isLiteral));
            base.Visit(node);
        }

        public override void Visit(SelectStarExpression node)
        {
            ReturnValues.Add(new SelectElementWrapper(node, _queryExpressionId, string.Empty, false));
            base.Visit(node);
        }
    }
}
