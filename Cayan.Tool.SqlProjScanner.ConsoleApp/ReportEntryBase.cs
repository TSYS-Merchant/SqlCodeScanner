namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    public class ReportEntryBase
    {
        public string Db { get; set; }

        public string Schema { get; set; }

        public string SpName { get; set; }

        public string SpUniqueName => $"{Db}\\{Schema}\\{SpName}";

        public ReportEntryBase()
        {

        }

        public ReportEntryBase(string db, string schema, string spName)
        {
            Db = db;
            Schema = schema;
            SpName = spName;
        }

    }
}
