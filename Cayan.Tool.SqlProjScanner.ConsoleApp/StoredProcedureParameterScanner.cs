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
                var length = ParseLength(p);

                var entry =
                    new ParamSqlReportEntry(p.VariableName.Value,
                        p.DataType.Name.BaseIdentifier.Value,
                        length,
                        isDefaulted);

                spReport.Parameters.Add(entry);
            }
        }

        private string ParseLength(ProcedureParameter p)
        {
            if (p.DataType is ParameterizedDataTypeReference parameterReference)
            {
                return parameterReference.Parameters.Count > 0 ? parameterReference.Parameters[0].Value : "N/A";
            }

            return "N/A";
        }
    }
}
