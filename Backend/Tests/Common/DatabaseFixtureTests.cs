using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using Tests.Infra.Common;

namespace Tests.Common
{
    [TestClass]
    public class DatabaseFixtureTests : DatabaseTestsBase
    {
        [TestMethod]
        public void InitializeTestDatabase_should_open_connection_and_create_new_database()
        {
            var initialDatabaseCount = GetAllNpgsqlDatabasesList().ToList().Count();

            // Act
            InitializeTestDatabase();
            var afterDatabaseCount = GetAllNpgsqlDatabasesList().ToList().Count();
            var databaseName = DatabaseFixture.NpgsqlConnection.Database;

            // Assert
            Assert.AreEqual(DatabaseFixture.NpgsqlConnection.State.ToString().ToLower(), "open");
            Assert.AreEqual(afterDatabaseCount, initialDatabaseCount+1);
            Assert.IsTrue(GetAllNpgsqlDatabasesList().ToList().Contains(databaseName));
        }

        [TestMethod]
        public void Dispose_should_remove_created_database_and_close_connection()
        {
            // Act
            InitializeTestDatabase();
            var initialDatabaseCount = GetAllNpgsqlDatabasesList().ToList().Count();
            var databaseName = DatabaseFixture.NpgsqlConnection.Database;
            DatabaseFixture.Dispose();
            var afterDatabaseCount = GetAllNpgsqlDatabasesList().ToList().Count();

            // Assert
            Assert.AreEqual(DatabaseFixture.NpgsqlConnection.State.ToString().ToLower(), "closed");
            Assert.AreEqual(afterDatabaseCount, initialDatabaseCount - 1);
            Assert.IsFalse(GetAllNpgsqlDatabasesList().ToList().Contains(databaseName));
        }

        [TestMethod]
        public void Old_databases_should_be_deleted()
        {
            // Act
            InitializeTestDatabase();
            var databases = GetAllNpgsqlDatabasesList().ToList();

            // Assert
            Assert.AreEqual(databases.Where(x => x.Contains("tests-db")).Count(), 1);
        }

        public static IEnumerable<string>  GetAllNpgsqlDatabasesList()
        {
            NpgsqlConnection connection = new NpgsqlConnection("server=localhost;Username=postgres;Password=parool;");
            connection.Open();
            var databaseCommand = new NpgsqlCommand("SELECT datname FROM pg_database", connection);

            NpgsqlDataReader dataReader = databaseCommand.ExecuteReader();

            while (dataReader.Read())
            {
                yield return dataReader.GetString(0);
            }

            connection.Close();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseFixture.Dispose();
        }
    }
}
