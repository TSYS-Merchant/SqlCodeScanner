namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    public class XmlStreamWrapperFactory : IXmlStreamWrapperFactory
    {
        public IXmlStreamWriterWrapper CreateXmlWriter(string xmlFilePath)
        {
            return new XmlStreamWriterWrapper(xmlFilePath);
        }
    }
}
