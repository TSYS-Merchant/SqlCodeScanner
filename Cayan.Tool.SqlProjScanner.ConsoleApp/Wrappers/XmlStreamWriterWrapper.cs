namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    using ReportObjects;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlStreamWriterWrapper : IXmlStreamWriterWrapper
    {
        private readonly XmlWriter _xmlWriter;
        private readonly XmlSerializerNamespaces _ns;

        public XmlStreamWriterWrapper(string xmlFilePath)
        {
            var writerSettings = new XmlWriterSettings
            {
                NewLineOnAttributes = true,
                Indent = true
            };

            _xmlWriter = XmlWriter.Create(
                xmlFilePath, writerSettings);

            _ns = new XmlSerializerNamespaces();
            _ns.Add("", "");
        }

        public void Dispose()
        {
            _xmlWriter.Dispose();
        }

        public void WriteStartElement(string element)
        {
            _xmlWriter.WriteStartElement(element);
        }

        public void WriteElementString(string element, string value)
        {
            _xmlWriter.WriteElementString(element, value);
        }

        public void WriteEndElement()
        {
            _xmlWriter.WriteEndElement();
        }

        public void Flush()
        {
            _xmlWriter.Flush();
        }

        public void SerializeSqlReportElement(StoredProcedureReport entry)
        {
            var paramSerializer =
                new XmlSerializer(typeof(StoredProcedureReport));

            paramSerializer.Serialize(_xmlWriter, entry, _ns);
        }
    }
}
