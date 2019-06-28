namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using ReportObjects;
    using System;
    using System.Collections.Generic;
    using Wrappers;

    [TestFixture]
    public class SqlFileScannerTests
    {

        [Test]
        public void OrchestrateSqlReport_ForCreateMasterReport_CreatesMasterReport()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Any<StoredProcedureReport>());
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForCreateMasterReportWithUnionSp_CreatesMasterReport()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UnionSp.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.UnionSp
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Is<StoredProcedureReport>(
                x => x.Parameters.Count == 2));
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Is<StoredProcedureReport>(
                x => x.ReturnValues.Count == 4));
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForCreateMasterReportWithStar_CreatesMasterReport()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UnionSp.StarTestWithStar",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.StarTestWithStar
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Any<StoredProcedureReport>());
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForCreateMasterReportNoBegin_CreatesMasterReport()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "NoBeginSp.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.NoBeginSp
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Any<StoredProcedureReport>());
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForNoSqlFilesInFolder_Ignores()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var nonSqlDirectory = Substitute.For<IDirectoryInfoWrapper>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var directoryWrapper = Substitute.For<IDirectoryWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            sqlDirectories.Add(nonSqlDirectory);

            directoryWrapper.GetDirectories(Arg.Any<string>())
                .Returns(sqlDirectories);

            var directoryFactory
                = Substitute.For<IDirectoryWrapperFactory>();

            directoryFactory.CreateDirectoryWrapper(Arg.Any<string>())
                .Returns(directoryWrapper);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.DidNotReceive().SerializeSqlReportElement(Arg.Any<StoredProcedureReport>());
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_MissingDirectoryPath_ThrowsException()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => scanner.OrchestrateSqlReport(
                    null, "reports\\data.xml",
                    null,
                    true));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("solution-path cannot be null or empty."));

            fileWrapper.DidNotReceive().ReadAllText(Arg.Any<string>());
            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_MissingDatafilePath_ThrowsException()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => scanner.OrchestrateSqlReport(
                    "path\\sql", null,
                    null,
                    true));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("data-file path cannot be null or empty."));

            fileWrapper.DidNotReceive().ReadAllText(Arg.Any<string>());
            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_MissingReportPath_ThrowsException()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => scanner.OrchestrateSqlReport(
                    "path\\sql", "reports\\data.xml",
                    null,
                    false));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("report path cannot be null or empty."));

            fileWrapper.DidNotReceive().ReadAllText(Arg.Any<string>());
            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForDifferentCaseInFileVsScript_GetsCorrectCase()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "FindUserWithID.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());

            xmlWriter.Received(1).SerializeSqlReportElement(
                Arg.Is<StoredProcedureReport>(
                    x => x.SpName == "FindUserWithId"));

            xmlWriter.Received(1).SerializeSqlReportElement(
                Arg.Is<StoredProcedureReport>(
                    x => x.SpName == "FindUserWithId"));

            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForDoNotCreateMasterReport_DoesComparison()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            fileWrapper.ReadAllText("reports\\data.xml").Returns(SqlSamples.ReportSampleXml);
            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner = new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                "reports\\scanResult.html",
                false);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.Received(1).ReadAllText("reports\\data.xml");
            htmlReportGenerator.Received(1).GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForComplexQuery_ParsesReturnValuesCorrectly()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            fileWrapper.ReadAllText("reports\\data.xml").Returns(SqlSamples.ReportSampleXml);
            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "FindUserByUserName.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserByUsername
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner = new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                "reports\\scanResult.html",
                false);

            // Assert
            Assert.That(result, Is.EqualTo(true));
            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.Received(1).ReadAllText("reports\\data.xml");
            htmlReportGenerator.Received(1).GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForOutput_ParsesOutputValuesCorrectly()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            fileWrapper.ReadAllText("reports\\data.xml").Returns(SqlSamples.ReportSampleXml);
            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "RetrieveOneUseCoupon.sql",
                        "path\\sql\\theDB\\Schema55\\Stored Procedures\\",
                        SqlSamples.RetrieveOneUseCoupon
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner = new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                "reports\\scanResult.html",
                false);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.Received(1).ReadAllText("reports\\data.xml");
            htmlReportGenerator.Received(1).GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }


        [Test]
        public void OrchestrateSqlReport_ForMissingParameter_FailsScan()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            fileWrapper.ReadAllText("reports\\data.xml").Returns(SqlSamples.ReportSampleXml);
            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "UserSearchByUserKey.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.FindUserWithId
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            paramReportComparer
                .When(x => x.CompareReports(Arg.Any<SqlReport>(), Arg.Any<SqlReport>(),
                    Arg.Any<List<string>>()))
                .Do(x =>
                {
                    var errors = (List<string>)x.Args()[2];
                    errors.Add("SomeId|param is defaulted in master but not in new code");
                });

            var scanner = new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer,
                returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                "reports\\scanResult.html",
                false);

            // Assert
            Assert.That(result, Is.EqualTo(false));

            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.Received(1).ReadAllText("reports\\data.xml");
            htmlReportGenerator.Received(1).GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForNoParameters_DoesNotThrowException()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "NoParametersOrReturnValues.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.NoParametersOrReturnValues
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer,
                    returnReportComparer);

            // Act
            var result = scanner.OrchestrateSqlReport(
                "path\\sql", "reports\\data.xml",
                null,
                true);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            xmlWriter.Received(1).WriteStartElement(Arg.Any<string>());
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Is<StoredProcedureReport>(
                x => x.Parameters.Count == 0));
            xmlWriter.Received(1).SerializeSqlReportElement(Arg.Is<StoredProcedureReport>(
                x => x.ReturnValues.Count == 0));
            xmlWriter.Received(1).WriteEndElement();
            xmlWriter.Received(1).WriteEndElement();
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        [Test]
        public void OrchestrateSqlReport_ForCannotParseSql_ThrowsException()
        {
            // Setup
            var fileWrapper = Substitute.For<IFileWrapper>();
            var xmlWriter = Substitute.For<IXmlStreamWriterWrapper>();
            var xmlWrapper = Substitute.For<IXmlStreamWrapperFactory>();
            var sqlDirectories = new List<IDirectoryInfoWrapper>();
            var htmlReportGenerator = Substitute.For<IHtmlReportGenerator>();
            var paramReportComparer = Substitute.For<IParamReportComparer>();
            var returnReportComparer = Substitute.For<IReturnReportComparer>();

            xmlWrapper.CreateXmlWriter(Arg.Any<string>()).Returns(xmlWriter);

            var sqlFileData = new List<List<List<string>>>
            {
                new List<List<string>>
                {
                    new List<string>
                    {
                        "CannotParseThis.sql",
                        "path\\sql\\theDB\\dbo\\Stored Procedures\\",
                        SqlSamples.CannotParseThis
                    }
                },
                new List<List<string>>()
            };

            var directoryFactory =
                SimulateSqlFiles("path\\sql", sqlFileData, fileWrapper, sqlDirectories);

            var scanner =
                new SqlFileScanner(fileWrapper, xmlWrapper, directoryFactory, htmlReportGenerator, paramReportComparer, returnReportComparer);

            // Act
            var ex = Assert.Throws<Exception>(
                () => scanner.OrchestrateSqlReport(
                    "path\\sql", "reports\\data.xml",
                    null,
                    true));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Error parsing SQL file path\\sql\\theDB\\dbo\\Stored Procedures\\CannotParseThis.sql"));
            fileWrapper.Received().ReadAllText("path\\sql\\theDB\\dbo\\Stored Procedures\\CannotParseThis.sql");
            xmlWriter.DidNotReceive().WriteStartElement(Arg.Any<string>());
            fileWrapper.DidNotReceive().ReadAllText("reports\\data.xml");
            htmlReportGenerator.DidNotReceive().GenerateComparisonReport(Arg.Any<string>(), Arg.Any<List<string>>());
        }

        private IDirectoryWrapperFactory SimulateSqlFiles(
            string path,
            List<List<List<string>>> sqlData, IFileWrapper fileWrapper,
            List<IDirectoryInfoWrapper> sqlDirectories)
        {
            var directoryFactory = Substitute.For<IDirectoryWrapperFactory>();

            // Get each directory
            foreach (var directory in sqlData)
            {
                var sqlFileList = new List<IFileInfoWrapper>();
                var sqlDir = Substitute.For<IDirectoryInfoWrapper>();

                // Get each SQL file in that directory
                foreach (var file in directory)
                {
                    var fileName = file[0];
                    var filePath = $"{file[1]}{fileName}";
                    var content = file[2];

                    var sqlFile1 = Substitute.For<IFileInfoWrapper>();
                    sqlFile1.FullName.Returns(filePath);

                    fileWrapper.ReadAllText(filePath)
                        .Returns(content);

                    sqlFileList.Add(sqlFile1);
                }

                // Fill the directory with those files
                sqlDir.GetFiles("*.sql").Returns(sqlFileList);
                sqlDirectories.Add(sqlDir);
            }

            var directoryWrapper = Substitute.For<IDirectoryWrapper>();
            directoryWrapper.GetDirectories("*Stored Procedures*")
                .Returns(sqlDirectories);

            directoryFactory.CreateDirectoryWrapper(path)
                .Returns(directoryWrapper);

            return directoryFactory;
        }
    }
}