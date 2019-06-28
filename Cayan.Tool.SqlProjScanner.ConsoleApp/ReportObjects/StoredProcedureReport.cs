namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class StoredProcedureReport
    {
        public string Db { get; set; }

        public string Schema { get; set; }

        public string SpName { get; set; }

        public string SpUniqueName => $"{Db}\\{Schema}\\{SpName}";

        [XmlElement("ParamSqlReportEntry")]
        public List<ParamSqlReportEntry> Parameters { get; set; }

        [XmlElement("ReturnSqlReportEntry")]
        public List<ReturnSqlReportEntry> ReturnValues { get; set; }

        public StoredProcedureReport()
        {
            
        }

        public StoredProcedureReport(string db, string schema, string spName)
        {
            Parameters = new List<ParamSqlReportEntry>();
            ReturnValues = new List<ReturnSqlReportEntry>();

            Db = db;
            Schema = schema;
            SpName = spName;
        }
    }
}
