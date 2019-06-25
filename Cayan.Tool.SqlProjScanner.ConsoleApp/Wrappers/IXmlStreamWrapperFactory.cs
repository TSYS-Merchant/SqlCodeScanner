namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Wrappers
{
    public interface IXmlStreamWrapperFactory
    {
        IXmlStreamWriterWrapper CreateXmlWriter(string xmlFilePath);
    }
}
