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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.OtherValue"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database3:schema1:StoredProcedure2:C.Id")
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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name")
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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Phone")
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForNewValueInMiddle_ReturnsError()
        {
            // Setup
            var returnComparer = new ReturnReportComparer();
            var errors = new List<string>();

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:[FieldB] = CAST(SUBSTRING(ISNULL([SomeTable].[text],''), 1, 1024) AS VARCHAR(1024))"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.SomeAmount"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Name"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.NewValue"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Id"),
                    MakeReturn("Database2:schema1:StoredProcedure2:C.Phone")
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

            var masterReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:*")
                }
            };

            var newReport = new SqlReport
            {
                ReturnValues = new List<ReturnSqlReportEntry>
                {
                    MakeReturn("Database1:dbo:StoredProcedure1:Param1"),
                    MakeReturn("Database1:dbo:StoredProcedure1:Param2"),
                    MakeReturn("Database1:dbo:StoredProcedure1:Param2")
                }
            };

            // Act
            returnComparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(2));
            Assert.That(errors[0], Is.EqualTo("Database1\\dbo\\StoredProcedure1\\*|existing return value is missing from new code"));
            Assert.That(errors[1], Is.EqualTo("Database1\\dbo\\StoredProcedure1\\*|existing return value is out of order"));
        }

        private ReturnSqlReportEntry MakeReturn(string entry)
        {
            var entryItems = entry.Split(':');

            return new ReturnSqlReportEntry()
            {
                Db = entryItems[0],
                Schema = entryItems[1],
                SpName = entryItems[2],
                ReturnValueName = entryItems[3]
            };
        }
    }
}
