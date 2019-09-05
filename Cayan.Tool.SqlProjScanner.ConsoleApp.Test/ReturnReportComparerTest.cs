namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using ReportObjects;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ReturnReportComparerTest
    {
        [Test]
        public void CompareReports_ForSameReports_ReturnsNoErrors()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                   sp1,
                   sp2
                }
            };

            var sp3 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp4 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3,
                    sp4
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForSameSpNameInDifferentDb_ReturnsNoErrors()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var sp3 = new StoredProcedureReport("Database3", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1,
                    sp2,
                    sp3
                }
            };

            var sp4 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp5 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var sp6 = new StoredProcedureReport("Database3", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp4,
                    sp5,
                    sp6
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForMissingValue_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1,
                    sp2
                }
            };

            var sp3 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp4 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3,
                    sp4
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("Database2\\schema1\\StoredProcedure2\\C.Name|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForReorderedValues_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1,
                    sp2
                }
            };

            var sp3 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp4 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3,
                    sp4
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(2));
            Assert.That(errors[0], Is.EqualTo("Database2\\schema1\\StoredProcedure2\\C.Name|existing return value is out of order"));
            Assert.That(errors[1], Is.EqualTo("Database2\\schema1\\StoredProcedure2\\C.Id|existing return value is out of order"));
        }

        [Test]
        public void CompareReports_ForNewValueAtEnd_ReturnsNoErrors()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1,
                    sp2
                }
            };

            var sp3 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp4 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Phone", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3,
                    sp4
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForMultipleSelectsWithNewValuesAtEnd_ReturnsNoErrors()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.SomeAmountExt", 2, false),
                    new ReturnSqlReportEntry("C.NameExt", 2, false),
                    new ReturnSqlReportEntry("C.IdExt", 2, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.SomethingElse", 1, false),
                    new ReturnSqlReportEntry("C.SomeAmountExt", 2, false),
                    new ReturnSqlReportEntry("C.NameExt", 2, false),
                    new ReturnSqlReportEntry("C.IdExt", 2, false),
                    new ReturnSqlReportEntry("C.SomethingElseExt", 2, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForRenamedField_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Phone AS ThePhoneNumber", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Phone AS DifferentPhoneNumber", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\StoredProcedure1\\C.Phone AS ThePhoneNumber|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForChangeFieldCase_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Phone", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.PhonE", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\StoredProcedure1\\C.Phone|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForNewValueInMiddle_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp2 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1,
                    sp2
                }
            };

            var sp3 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry(
                        "[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, false, 1, false024) AS VARCHAR(1024))", 1, false)
                }
            };

            var sp4 = new StoredProcedureReport("Database2", "schema1", "StoredProcedure2")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.SomeAmount", 1, false),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.NewValue", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false),
                    new ReturnSqlReportEntry("C.Phone", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3,
                    sp4
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("Database2\\schema1\\StoredProcedure2\\C.Id|existing return value is out of order"));
        }

        [Test]
        public void CompareReports_StarReplacesWithParamNames_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("*", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("Param1", 1, false),
                    new ReturnSqlReportEntry("Param2", 1, false),
                    new ReturnSqlReportEntry("Param3", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("Database1\\dbo\\StoredProcedure1\\*|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_SelectStatementRemoved_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("A.Name1", 1, false),
                    new ReturnSqlReportEntry("A.Name2", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("Database1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(2));
            Assert.That(errors[0], Is.EqualTo("Database1\\dbo\\StoredProcedure1\\A.Name1|existing return value is missing from new code"));
            Assert.That(errors[1], Is.EqualTo("Database1\\dbo\\StoredProcedure1\\A.Name2|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForChangeStringLiteral_ReturnsNoError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello'", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello 123'", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForRemoveStringLiteral_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello'", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\StoredProcedure1\\'Hello'|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForReplaceColumnWithLiteral_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("Hello", 1, true),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\StoredProcedure1\\C.Name|existing return value is missing from new code"));
        }

        [Test]
        public void CompareReports_ForRenameStringLiteral_ReturnsNoError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello' AS TheName", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello' AS DifferentName", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForRemoveNameOnStringLiteral_ReturnsNoError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello' AS TheName", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "StoredProcedure1")
            {
                ReturnValues = new List<ReturnSqlReportEntry>()
                {
                    new ReturnSqlReportEntry("'Hello'", 1, true),
                    new ReturnSqlReportEntry("C.Name", 1, false),
                    new ReturnSqlReportEntry("C.Id", 1, false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }
    }
}
