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

            var storedProcedureReport = new StoredProcedureReport()
            {
                Db = "MyDB",
                Schema = "Schema1",
                SpName = "SomeSp1",
                Parameters = new List<ParamSqlReportEntry>()
                {
                    new ParamSqlReportEntry("@Id", false)
                },
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("P.Name"),
                }
            };

            var sqlReport = new SqlReport
            {
                TimeStamp = "6/21/2019 6:45:00 PM",
                StoredProcedures = new List<StoredProcedureReport>()
                {
                    storedProcedureReport
                }
            };

            var writer = new SqlReportWriter(xmlFactory, fileWrapper);

            // Act
            writer.WriteMasterReport("c:\\output\\fakeReport1.xml",
                sqlReport);

            // Assert
            streamWriter.Received(1).WriteStartElement("SqlReport");
            streamWriter.Received(1).WriteElementString("TimeStamp", Arg.Any<string>());
            streamWriter.Received(1).SerializeSqlReportElement(storedProcedureReport);
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
            Assert.That(report.StoredProcedures.Count, Is.EqualTo(1));
            Assert.That(report.StoredProcedures[0].Db, Is.EqualTo("Database1"));
            Assert.That(report.StoredProcedures[0].Schema, Is.EqualTo("dbo"));
            Assert.That(report.StoredProcedures[0].SpName, Is.EqualTo("LoadUserByName"));

            Assert.That(report.StoredProcedures[0].Parameters.Count, Is.EqualTo(1));
            Assert.That(report.StoredProcedures[0].ReturnValues.Count, Is.EqualTo(1));
            Assert.That(report.StoredProcedures[0].Parameters[0].IsDefaulted, Is.EqualTo(true));
            Assert.That(report.StoredProcedures[0].Parameters[0].ParameterName, Is.EqualTo("@UserName"));
            Assert.That(report.StoredProcedures[0].ReturnValues[0].ReturnValueName, Is.EqualTo("U.UserName"));
        }
    }
}
