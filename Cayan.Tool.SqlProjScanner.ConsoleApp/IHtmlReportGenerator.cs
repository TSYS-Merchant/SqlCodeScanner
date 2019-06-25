namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;

    public interface IHtmlReportGenerator
    {
        void GenerateComparisonReport(string reportPath, List<string> errors);
    }
}
