namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    public class ReturnSqlReportEntry : ReportEntryBase
    {
       public string ReturnValueName { get; set; }

       public string ReturnValueNameId => $"{Db}\\{Schema}\\{SpName}\\{ReturnValueName}";

       public ReturnSqlReportEntry()
       {

       }

        public ReturnSqlReportEntry(string db, string schema, string spName, string returnValueName)
           : base(db, schema, spName)
       {
           ReturnValueName = returnValueName;
       }
    }
}
