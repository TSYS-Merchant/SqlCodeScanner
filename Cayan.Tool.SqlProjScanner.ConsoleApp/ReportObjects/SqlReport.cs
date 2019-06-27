namespace Cayan.Tool.SqlProjScanner.ConsoleApp.ReportObjects
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SqlReport
    {
        [XmlElement("TimeStamp")]
        public string TimeStamp { get; set; }

        [XmlElement("ParamSqlReportEntry")]
        public List<ParamSqlReportEntry> Parameters { get; set; }

        [XmlElement("ReturnSqlReportEntry")]
        public List<ReturnSqlReportEntry> ReturnValues { get; set; }

        public SqlReport()
        {
            Parameters = new List<ParamSqlReportEntry>();
            ReturnValues = new List<ReturnSqlReportEntry>();
        }

    }
}
