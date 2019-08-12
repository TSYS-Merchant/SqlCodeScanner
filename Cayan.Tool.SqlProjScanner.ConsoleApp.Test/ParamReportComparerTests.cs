namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NUnit.Framework;
    using ReportObjects;
    using System.Collections.Generic;

    [TestFixture]
    public class ParamReportComparerTests
    {
        [Test]
        public void CompareReports_ForEqualReports_ReturnsNoError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForExistingParamDefaulted_ReturnsNoError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", true),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForNewDefault_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true),
                    new ParamSqlReportEntry("@username", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForSameSpNameInDifferentDb_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB2", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
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

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp4 = new StoredProcedureReport("DB2", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
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

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForNewNonDefaultInNewSp_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false)
                }
            };


            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2,
                    sp3
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForNoArityChange_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", "200", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", "200", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", "200", true)
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

            var sp4 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", "200", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", "200", false)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", "200", true)
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

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForRemovedParam_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@id|existing parameter is missing from new code"));
        }

        [Test]
        public void CompareReports_ForNoLongerDefaultedParam_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@data|parameter is defaulted in master but not in new code"));
        }

        [Test]
        public void CompareReports_ForNewNonDefault_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true),
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@name|new parameter has no default"));
        }

        [Test]
        public void CompareReports_ForBigIntToInt_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "BIGINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@id|existing BIGINT parameter was changed to INT"));
        }

        [Test]
        public void CompareReports_ForBigIntToIntWithCapitalization_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "bigint", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@id|existing BIGINT parameter was changed to INT"));
        }

        [Test]
        public void CompareReports_ForBigIntCapitalization_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "BIGINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "BIGINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForBigIntToTinyInt_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "bigint", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "TINYINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@id|existing BIGINT parameter was changed to TINYINT"));
        }

        [Test]
        public void CompareReports_ForIntToBigInt_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "BIGINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForBigIntRemoved_ReturnsCorrectError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "BIGINT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var masterReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp1
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                   new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp2
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething1\\@id|existing parameter is missing from new code"));
        }

        [Test]
        public void CompareReports_ForArityChange_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", "200", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", "200", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", "200", true)
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

            var sp4 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false),
                    new ParamSqlReportEntry("@phone", "VARCHAR", "200", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", "200", true)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", "200", true)
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

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(2));
            Assert.That(errors[0], Is.EqualTo("DB1\\dbo\\LoadSomething2\\@address|existing parameter is out of order"));
            Assert.That(errors[1], Is.EqualTo("DB1\\dbo\\LoadSomething2\\@phone|existing parameter is out of order"));
        }

        [Test]
        public void CompareReports_ForSpDeleted_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", "200", false)
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

            var sp4 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", "200", false)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp4,
                    sp5
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void CompareReports_ForSpCaseChanged_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", "200", false)
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

            var sp4 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "loadsomethinG2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", "200", false)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", "200", false)
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

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors[0],
                Is.EqualTo("DB1\\dbo\\LoadSomething2:loadsomethinG2\\|sp was renamed with different case"));
        }

        [Test]
        public void CompareReports_ForNewInMaster_ReturnsNoError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
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

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", "N/A", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", "200", true)
                }
            };

            var newReport = new SqlReport
            {
                StoredProcedures = new List<StoredProcedureReport>
                {
                    sp3
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }
    }
}
