namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ParamSqlReportEntry
    {

        public string ParameterName { get; set; }

        public string ParameterType { get; set; }

        public bool IsDefaulted { get; set; }

        public ParamSqlReportEntry()
        {

        }

        public ParamSqlReportEntry(string parameterName,
            string parameterType, bool isDefaulted)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
            IsDefaulted = isDefaulted;
        }
    }
}
