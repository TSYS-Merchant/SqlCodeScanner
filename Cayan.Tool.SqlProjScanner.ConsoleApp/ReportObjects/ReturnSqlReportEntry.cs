namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ReturnSqlReportEntry
    {
       public string ReturnValueName { get; set; }

       public ReturnSqlReportEntry()
       {

       }

        public ReturnSqlReportEntry(string returnValueName)
        {
           ReturnValueName = returnValueName;
       }
    }
}
