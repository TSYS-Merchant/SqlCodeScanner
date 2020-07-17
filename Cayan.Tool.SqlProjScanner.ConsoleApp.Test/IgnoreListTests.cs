namespace Cayan.Tool.SqlProjScanner.ConsoleApp.Test
{
    using NUnit.Framework;
    using Wrappers;
    using NSubstitute;
    using System.IO;
    using NSubstitute.ExceptionExtensions;

    [TestFixture]
    public class IgnoreListTests
    {
        [Test]
        public void IsSchemaIgnored_ForIsOnIgnoreList_DoesIgnore()
        {
            // Setup
            string[] ignoreSchemas = { "db1.Schema1", "db2.Schema2" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForNotOnIgnoreList_DoesNotIgnore()
        {
            // Setup
            string[] ignoreSchemas = { "db1.Schema1", "db2.Schema2" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "Schema2");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(false));
        }

        [Test]
        public void IsSchemaIgnored_ForCommentPresent_IgnoresComment()
        {
            // Setup
            string[] ignoreSchemas = { "-- A comment here", "db1.Schema1", "db2.Schema1", "-- Another comment" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForCommentAfterSpacePresent_IgnoresComment()
        {
            // Setup
            string[] ignoreSchemas = { " -- A comment here", "db1.Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForNumCharacterPresent_AllowsCharacter()
        {
            // Setup
            string[] ignoreSchemas = { "db1.#Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "#Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForDashCharacterPresent_AllowsCharacter()
        {
            // Setup
            string[] ignoreSchemas = { "db1.-Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "-Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_SchemaCommentedOut_IgnoresSchema()
        {
            // Setup
            string[] ignoreSchemas = { "-- db1.Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db1", "Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(false));
        }

        [Test]
        public void IsSchemaIgnored_SchemaCommentedOutWithSpace_IgnoresSchema()
        {
            // Setup
            string[] ignoreSchemas = { " -- db1.Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isSchemaIgnored = ignoreList.IsSchemaIgnored("db1", "Schema1");
            var isMalformedSchemaIgnored = ignoreList.IsSchemaIgnored(" -- db1", "Schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isSchemaIgnored, Is.EqualTo(false));
            Assert.That(isMalformedSchemaIgnored, Is.EqualTo(false));
        }

        [Test]
        public void IsSchemaIgnored_ForVariantCase_IgnoresCase()
        {
            // Setup
            string[] ignoreSchemas = { "Db1.Schema1", "db2.Schema1" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isUpperIgnored = ignoreList.IsSchemaIgnored("dB1", "ScHEma1");
            var isLowerIgnored = ignoreList.IsSchemaIgnored("db1", "schema1");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isUpperIgnored, Is.EqualTo(true));
            Assert.That(isLowerIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForMalformedEntry_DoesNotThrowException()
        {
            // Setup
            string[] ignoreSchemas = { "db1.Schema1", "db2Schema2", "db2.Schema2" };
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(true);
            fileWrapper.ReadLines(ignoreFile)
                .Returns(ignoreSchemas);

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db2", "Schema2");

            // Assert
            fileWrapper.Received(1).ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(true));
        }

        [Test]
        public void IsSchemaIgnored_ForFileNotFound_IgnoresAndContinues()
        {
            // Setup
            var ignoreFile = "SchemaIgnore.txt";

            var fileWrapper = Substitute.For<IFileWrapper>();

            fileWrapper.Exists(ignoreFile).Returns(false);
            fileWrapper.ReadLines(ignoreFile)
                .Throws(new FileNotFoundException());

            var ignoreList = new IgnoreList(fileWrapper, ignoreFile);

            // Act
            var isIgnored = ignoreList.IsSchemaIgnored("db2", "Schema2");

            // Assert
            fileWrapper.DidNotReceive().ReadLines(ignoreFile);
            Assert.That(isIgnored, Is.EqualTo(false));
        }
    }
}