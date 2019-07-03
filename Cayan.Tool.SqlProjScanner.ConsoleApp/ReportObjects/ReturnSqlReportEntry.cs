namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ReturnSqlReportEntry
    {
       public string ReturnValueName { get; set; }

       public int StatementId { get; set; }

       public ReturnSqlReportEntry()
       {

       }

        public ReturnSqlReportEntry(string returnValueName,
            int statementId)
        {
           ReturnValueName = returnValueName;
           StatementId = statementId;
        }
    }
}
