namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class ReturnSqlReportEntry
    {
        /// <summary>
        /// The value being returned, for example U.UserId
        /// </summary>
       public string ReturnValueName { get; set; }

       public int StatementId { get; set; }

       public bool IsLiteral { get; set; }

       /// <summary>
       /// Only present if 'AS' or '=' are used
       /// </summary>
        public string ColumnNamedAs { get; set; }

       public ReturnSqlReportEntry()
       {

       }

        public ReturnSqlReportEntry(string returnValueName,
            int statementId, bool isLiteral, string columnNamedAs)
        {
           ReturnValueName = returnValueName;
           StatementId = statementId;
           IsLiteral = isLiteral;
           ColumnNamedAs = columnNamedAs;
        }
    }
}
