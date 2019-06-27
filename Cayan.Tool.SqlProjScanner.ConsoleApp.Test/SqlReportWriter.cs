namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using ReportObjects;
    using System.Collections.Generic;
    using Wrappers;

    [TestFixture]
    public class SqlReportWriterTests
    {
        [Test]
        public void WriteMasterReport_ForValidReport_WritesDataToFile()
        {
            // Setup
            var xmlFactory = Substitute.For<IXmlStreamWrapperFactory>();
            var fileWrapper = Substitute.For<IFileWrapper>();
            var streamWriter = Substitute.For<IXmlStreamWriterWrapper>();

            xmlFactory.CreateXmlWriter("c:\\output\\fakeReport1.xml")
                .Returns(streamWriter);

            var returnEntry = new ReturnSqlReportEntry
            {
                ReturnValueName = "P.Name",
                Db = "MyDB",
                Schema = "Schema1",
                SpName = "SomeSp1"
            };

            var parameterEntry = new ParamSqlReportEntry
            {
                ParameterName = "@Id",
                IsDefaulted = false,
                Db = "MyDB",
                Schema = "Schema1",
                SpName = "SomeSp1"
            };

            var sqlReport = new SqlReport
            {
                TimeStamp = "6/21/2019 6:45:00 PM",
                ReturnValues = new List<ReturnSqlReportEntry> {returnEntry},
                Parameters = new List<ParamSqlReportEntry> {parameterEntry}
            };

            var writer = new SqlReportWriter(xmlFactory, fileWrapper);

            // Act
            writer.WriteMasterReport("c:\\output\\fakeReport1.xml",
                sqlReport);

            // Assert
            streamWriter.Received(1).WriteStartElement("SqlReport");
            streamWriter.Received(1).WriteElementString("TimeStamp", Arg.Any<string>());
            streamWriter.Received(1).SerializeSqlReportElement(parameterEntry);
            streamWriter.Received(1).SerializeSqlReportElement(returnEntry);
            streamWriter.Received(1).WriteEndElement();
            streamWriter.Received(1).Flush();
        }

        [Test]
        public void LoadMasterReport_ForValidReport_LoadsData()
        {
            // Setup
            var xmlFactory = Substitute.For<IXmlStreamWrapperFactory>();
            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.ReadAllText("c:\\output\\fakeReport1.xml")
                .Returns(SqlSamples.ReportSampleXml);

            var writer = new SqlReportWriter(xmlFactory, fileWrapper);

            // Act
            var report = 
                writer.LoadMasterReport("c:\\output\\fakeReport1.xml");

            // Assert
            Assert.That(report.TimeStamp, Is.EqualTo("6/24/2019 5:44:53 PM"));
            Assert.That(report.Parameters.Count, Is.EqualTo(1));
            Assert.That(report.ReturnValues.Count, Is.EqualTo(1));

            Assert.That(report.Parameters[0].IsDefaulted, Is.EqualTo(true));
            Assert.That(report.Parameters[0].Db, Is.EqualTo("Database1"));
            Assert.That(report.Parameters[0].Schema, Is.EqualTo("dbo"));
            Assert.That(report.Parameters[0].SpName, Is.EqualTo("LoadUserByName"));
            Assert.That(report.Parameters[0].ParameterName, Is.EqualTo("@UserName"));

            Assert.That(report.ReturnValues[0].Db, Is.EqualTo("Database1"));
            Assert.That(report.ReturnValues[0].Schema, Is.EqualTo("dbo"));
            Assert.That(report.ReturnValues[0].SpName, Is.EqualTo("LoadUserByName"));
            Assert.That(report.ReturnValues[0].ReturnValueName, Is.EqualTo("U.UserName"));
        }
    }
}
