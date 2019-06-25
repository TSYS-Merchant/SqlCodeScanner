namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;

    public interface IReturnReportComparer
    {
        void CompareReports(SqlReport masterReport, SqlReport newReport, List<string> errors);
    }
}
