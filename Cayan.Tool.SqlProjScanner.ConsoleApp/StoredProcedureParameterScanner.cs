namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;

    public class StoredProcedureParameterScanner
    {
        public void ScanSpParameters(SqlReport sqlReport,
            CreateProcedureStatement sp, string db, string schema)
        {

            if (sp.Parameters.Count == 0)
            {
                return;
            }

            foreach (var p in sp.Parameters)
            {
                var spFormattedName =
                    sp.ProcedureReference.Name.BaseIdentifier.Value;
                var isDefaulted = p.Value != null;

                var entry =
                    new ParamSqlReportEntry(db, schema, spFormattedName, p.VariableName.Value, isDefaulted);

                sqlReport.Parameters.Add(entry);
            }
        }
    }
}
