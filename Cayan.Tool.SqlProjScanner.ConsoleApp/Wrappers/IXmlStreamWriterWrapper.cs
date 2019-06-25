namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using System;

    public interface IXmlStreamWriterWrapper : IDisposable
    {
        void WriteStartElement(string element);

        void WriteElementString(string element, string value);

        void WriteEndElement();

        void Flush();

        void SerializeSqlReportElement(ParamSqlReportEntry entry);

        void SerializeSqlReportElement(ReturnSqlReportEntry entry);
    }
}
