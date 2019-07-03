namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class SelectElementVisitor : TSqlFragmentVisitor
    {
        public readonly List<SelectElement> ReturnValues;

        public SelectElementVisitor()
        {
            ReturnValues = new List<SelectElement>();
        }

        public override void Visit(SelectScalarExpression node)
        {
            ReturnValues.Add(node);
            base.Visit(node);
        }

        public override void Visit(SelectStarExpression node)
        {
            ReturnValues.Add(node);
            base.Visit(node);
        }
    }
}
