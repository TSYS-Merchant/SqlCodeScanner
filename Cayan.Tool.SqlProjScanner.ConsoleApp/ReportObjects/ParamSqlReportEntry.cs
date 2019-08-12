namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ParamSqlReportEntry
    {

        public string ParameterName { get; set; }

        public string ParameterType { get; set; }

        public string Length { get; set; }

        public bool IsDefaulted { get; set; }

        public ParamSqlReportEntry()
        {

        }

        public ParamSqlReportEntry(string parameterName,
            string parameterType, string length, bool isDefaulted)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
            Length = length;
            IsDefaulted = isDefaulted;
        }
    }
}
