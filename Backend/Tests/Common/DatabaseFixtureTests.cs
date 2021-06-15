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
            var initialDatabaseCount = GetAllNpgsqlDatabaseNames().ToList().Count();

            // Act
            InitializeTestDatabase();
            var afterDatabaseCount = GetAllNpgsqlDatabaseNames().ToList().Count();
            var databaseName = DatabaseFixture.NpgsqlConnection.Database;

            // Assert
            Assert.AreEqual(DatabaseFixture.NpgsqlConnection.State.ToString().ToLower(), "open");
            Assert.AreEqual(afterDatabaseCount, initialDatabaseCount+1);
            Assert.IsTrue(GetAllNpgsqlDatabaseNames().ToList().Contains(databaseName));
        }

        [TestMethod]
        public void Dispose_should_remove_created_database_and_close_connection()
        {
            // Act
            InitializeTestDatabase();
            var initialDatabaseCount = GetAllNpgsqlDatabaseNames().ToList().Count();
            var databaseName = DatabaseFixture.NpgsqlConnection.Database;
            DatabaseFixture.Dispose();
            var finalDatabaseCount = GetAllNpgsqlDatabaseNames().ToList().Count();

            // Assert
            Assert.AreEqual(DatabaseFixture.NpgsqlConnection.State.ToString().ToLower(), "closed");
            Assert.AreEqual(finalDatabaseCount, initialDatabaseCount - 1);
            Assert.IsFalse(GetAllNpgsqlDatabaseNames().ToList().Contains(databaseName));
        }

        [TestMethod]
        public void Old_databases_should_be_deleted()
        {
            // Act
            InitializeTestDatabase();
            var databases = GetAllNpgsqlDatabaseNames().ToList();

            // Assert
            Assert.AreEqual(1,databases.Count(x => x.Contains("tests-db")));
        }

        public static IEnumerable<string>  GetAllNpgsqlDatabaseNames()
        {
            var connection = new NpgsqlConnection("server=localhost;Username=postgres;Password=parool;");
            connection.Open();
            var databaseCommand = new NpgsqlCommand("SELECT datname FROM pg_database", connection);

            var dataReader = databaseCommand.ExecuteReader();

            while (dataReader.Read())
            {
                yield return dataReader.GetString(0);
            }

            connection.Close();
        }
    }
}
