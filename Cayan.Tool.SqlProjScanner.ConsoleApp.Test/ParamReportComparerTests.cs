namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ParamReportComparerTests
    {
        [Test]
        public void CompareReports_ForEqualReports_ReturnsNoError()
        {
            // Setup
            var comparer = new ParamReportComparer();
            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
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
            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething1.@username", true)
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
            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB2.dbo.LoadSomething1.@id", false),
                    MakeParam("DB2.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething1.@username", true),
                    MakeParam("DB2.dbo.LoadSomething1.@id", false),
                    MakeParam("DB2.dbo.LoadSomething1.@data", true)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@id", false)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething2.@address", true),
                    MakeParam("DB1.dbo.LoadSomething2.@phone", false),
                    MakeParam("DB1.dbo.LoadSomething3.@id", true),
                    MakeParam("DB1.dbo.LoadSomething3.@style", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething2.@address", true),
                    MakeParam("DB1.dbo.LoadSomething2.@phone", false),
                    MakeParam("DB1.dbo.LoadSomething3.@id", true),
                    MakeParam("DB1.dbo.LoadSomething3.@style", true),
                    MakeParam("DB1.dbo.LoadSomething4.@menu", false)
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
            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", false)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething1.@name", false)
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
        public void CompareReports_ForArityChange_ReturnsError()
        {
            // Setup
            var comparer = new ParamReportComparer();

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething2.@address", true),
                    MakeParam("DB1.dbo.LoadSomething2.@phone", false),
                    MakeParam("DB1.dbo.LoadSomething3.@id", true),
                    MakeParam("DB1.dbo.LoadSomething3.@style", true)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething2.@phone", false),
                    MakeParam("DB1.dbo.LoadSomething2.@address", true),
                    MakeParam("DB1.dbo.LoadSomething3.@id", true),
                    MakeParam("DB1.dbo.LoadSomething3.@style", true)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething3.@info", false)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething3.@info", false)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething3.@info", false)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.loadsomethinG2.@name", false),
                    MakeParam("DB1.dbo.LoadSomething3.@info", false)
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

            var masterReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true),
                    MakeParam("DB1.dbo.LoadSomething2.@id",false)
                }
            };

            var newReport = new SqlReport
            {
                Parameters = new List<ParamSqlReportEntry>
                {
                    MakeParam("DB1.dbo.LoadSomething1.@id", false),
                    MakeParam("DB1.dbo.LoadSomething1.@data", true)
                }
            };

            var errors = new List<string>();

            // Act
            comparer.CompareReports(masterReport, newReport, errors);

            // Assert
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        private ParamSqlReportEntry MakeParam(string entry, bool isDefaulted)
        {
            var entryItems = entry.Split('.');

            return new ParamSqlReportEntry
            {
                Db = entryItems[0],
                Schema = entryItems[1],
                SpName = entryItems[2],
                ParameterName = entryItems[3],
                IsDefaulted = isDefaulted
            };
        }
    }
}
