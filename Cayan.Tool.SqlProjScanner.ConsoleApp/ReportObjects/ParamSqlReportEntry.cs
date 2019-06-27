namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ParamSqlReportEntry : ReportEntryBase
    {

        public string ParameterName { get; set; }

        public bool IsDefaulted { get; set; }

        public string ParameterId => $"{Db}\\{Schema}\\{SpName}\\{ParameterName}";

        public ParamSqlReportEntry()
        {

        }

        public ParamSqlReportEntry(string db, string schema, string spName, string parameterName, bool isDefaulted)
            : base(db, schema, spName)
        {
            ParameterName = parameterName;
            IsDefaulted = isDefaulted;
        }
    }
}
