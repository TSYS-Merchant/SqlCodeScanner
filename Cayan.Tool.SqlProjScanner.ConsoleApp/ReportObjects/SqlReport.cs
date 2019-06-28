namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SqlReport
    {
        [XmlElement("TimeStamp")]
        public string TimeStamp { get; set; }

        [XmlElement("StoredProcedureReport")]
        public List<StoredProcedureReport> StoredProcedures { get; set; }

        public SqlReport()
        {
            StoredProcedures = new List<StoredProcedureReport>();
        }

    }
}
