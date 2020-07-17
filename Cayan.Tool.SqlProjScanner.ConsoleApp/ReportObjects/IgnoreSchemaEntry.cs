namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    public class IgnoreSchemaEntry
    {
        public string Db { get; set; }

        public string Schema { get; set; }

        public IgnoreSchemaEntry(string db, string schema)
        {
            Db = db;
            Schema = schema;
        }
    }
}
