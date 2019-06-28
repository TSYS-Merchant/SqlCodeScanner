namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class SelectAndDeleteVisitor : TSqlFragmentVisitor
    {
        public readonly List<TSqlFragment> SelectAndDeleteStatements;

        public SelectAndDeleteVisitor()
        {
            SelectAndDeleteStatements = new List<TSqlFragment>();
        }


        public override void Visit(SelectStatement node)
        {
            SelectAndDeleteStatements.Add(node);
            base.Visit(node);
        }

        public override void Visit(DeleteStatement node)
        {
            SelectAndDeleteStatements.Add(node);
            base.Visit(node);
        }

    }
}
