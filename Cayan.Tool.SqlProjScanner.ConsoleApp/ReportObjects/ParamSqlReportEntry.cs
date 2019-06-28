namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ParamSqlReportEntry
    {

        public string ParameterName { get; set; }

        public bool IsDefaulted { get; set; }

        public ParamSqlReportEntry()
        {

        }

        public ParamSqlReportEntry(string parameterName, bool isDefaulted)
        {
            ParameterName = parameterName;
            IsDefaulted = isDefaulted;
        }
    }
}
