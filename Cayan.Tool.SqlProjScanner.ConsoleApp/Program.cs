namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var sqlReportManager = new SqlReportConsoleManager();

            return sqlReportManager.MakeSqlReport(args);
        }
    }
}
