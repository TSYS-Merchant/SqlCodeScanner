namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    public class Configuration
    {
        public AppSettings AppSettings { get; set; }

        public Configuration()
        {
            AppSettings = new AppSettings();
        }
    }
}