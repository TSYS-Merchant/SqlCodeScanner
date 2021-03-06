﻿namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Wrappers;

    public class SqlReportWriter
    {
        private readonly IXmlStreamWrapperFactory _xmlWriterWrapperFactory;
        private readonly IFileWrapper _fileWrapper;

        public SqlReportWriter(IXmlStreamWrapperFactory xmlWriterWrapperFactory,
            IFileWrapper fileWrapper)
        {
            _xmlWriterWrapperFactory = xmlWriterWrapperFactory;
            _fileWrapper = fileWrapper;
        }

        public void WriteMasterReport(string dataFilePath, SqlReport masterReport)
        {
            using (var writer = _xmlWriterWrapperFactory.CreateXmlWriter(dataFilePath))
            {
                writer.WriteStartElement("SqlReport");
                writer.WriteElementString("TimeStamp", $"{DateTime.Now}");

                foreach (var sp in masterReport.StoredProcedures)
                {
                    writer.SerializeSqlReportElement(sp);
                }

                writer.WriteEndElement();
                writer.Flush();
            }

        }

        public SqlReport LoadMasterReport(string dataFilePath)
        {
            using (var stringReader = new StringReader(_fileWrapper.ReadAllText(dataFilePath)))
            {
                var deserializer = new XmlSerializer(typeof(SqlReport));

                return (SqlReport)deserializer.Deserialize(stringReader);
            }
        }
    }
}
