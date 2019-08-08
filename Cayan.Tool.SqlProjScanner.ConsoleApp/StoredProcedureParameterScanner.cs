namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using ReportObjects;

    public class StoredProcedureParameterScanner
    {
        public void ScanSpParameters(StoredProcedureReport spReport,
            CreateProcedureStatement sp)
        {

            if (sp.Parameters.Count == 0)
            {
                return;
            }

            foreach (var p in sp.Parameters)
            {
                var isDefaulted = p.Value != null;

                var entry =
                    new ParamSqlReportEntry(p.VariableName.Value,
                        p.DataType.Name.BaseIdentifier.Value,
                        isDefaulted);

                spReport.Parameters.Add(entry);
            }
        }
    }
}
