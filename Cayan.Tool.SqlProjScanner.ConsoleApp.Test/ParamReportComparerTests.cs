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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", true),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true),
                    new ParamSqlReportEntry("@username", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB2", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp4 = new StoredProcedureReport("DB2", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", false)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true),
                    new ParamSqlReportEntry("@name", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "BIGINT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
        public void CompareReports_ForIntToBigInt_DoesNotReturnError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var sp1 = new StoredProcedureReport("DB1", "dbo", "LoadSomething1")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "BIGINT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "BIGINT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                   new ParamSqlReportEntry("@data", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", true),
                    new ParamSqlReportEntry("@phone", "VARCHAR", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false),
                    new ParamSqlReportEntry("@phone", "VARCHAR", false),
                    new ParamSqlReportEntry("@address", "VARCHAR", true)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", true),
                    new ParamSqlReportEntry("@style", "VARCHAR", true)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false)
                }
            };

            var sp3 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp5 = new StoredProcedureReport("DB1", "dbo", "loadsomethinG2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@name", "VARCHAR", false)
                }
            };

            var sp6 = new StoredProcedureReport("DB1", "dbo", "LoadSomething3")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@info", "VARCHAR", false)
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
                }
            };

            var sp2 = new StoredProcedureReport("DB1", "dbo", "LoadSomething2")
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    new ParamSqlReportEntry("@id", "INT", false),
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
                    new ParamSqlReportEntry("@id", "INT", false),
                    new ParamSqlReportEntry("@data", "VARCHAR", true)
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
