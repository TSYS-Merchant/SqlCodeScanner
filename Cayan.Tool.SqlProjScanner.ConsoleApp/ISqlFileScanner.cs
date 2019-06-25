namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    public interface ISqlFileScanner
    {
        bool OrchestrateSqlReport(string sqlCodePath, string dataFilePath,
            string htmlReportPath, bool createDataFile);
    }
}
