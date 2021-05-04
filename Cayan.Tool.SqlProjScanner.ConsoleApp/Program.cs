namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static int Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var config = configBuilder.Get<Configuration>();

            var sqlReportManager = new SqlReportConsoleManager(config);

            return sqlReportManager.MakeSqlReport(args);
        }
    }
}
