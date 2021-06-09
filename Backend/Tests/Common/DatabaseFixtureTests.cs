using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Infra.Common;

namespace Tests.Common
{
    [TestClass]
    public class DatabaseFixtureTests : TestDatabaseInitializer
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }
        [TestMethod]
        public void DatabaseCreationTest()
        {

            Assert.AreEqual(_databaseFixture.NpgsqlConnection.State.ToString().ToLower(), "open");
            var dbList = GetAllNpgsqlDatabasesList().ToList();
            Assert.AreEqual(dbList.Count(), 1);
        }
        [TestMethod]
        public void DatabaseDisposeTest()
        {
            _databaseFixture.Dispose();
            Assert.AreEqual(_databaseFixture.NpgsqlConnection.State.ToString().ToLower(), "closed");
        }
        public static IEnumerable<string>  GetAllNpgsqlDatabasesList()
        {
            NpgsqlConnection connection = new NpgsqlConnection("server=localhost;Username=postgres;Password=parool;");
            connection.Open();
            var databaseCommand = new NpgsqlCommand("SELECT datname FROM pg_database", connection);

            NpgsqlDataReader dataReader = databaseCommand.ExecuteReader();

            string i;
            while (dataReader.Read())
            {
                i = dataReader.GetString(0);
                yield return i;
            }

        }
    }
}
