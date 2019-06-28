namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Collections.Generic;

    public interface IReturnReportComparer
    {
        void CompareReports(SqlReport masterReport, SqlReport newReport, List<string> errors);
    }
}
