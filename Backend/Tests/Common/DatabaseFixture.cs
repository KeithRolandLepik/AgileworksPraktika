using Npgsql;
using System;

namespace Tests.Common
{
    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get; private set; }
        private NpgsqlConnectionStringBuilder ConnectionStringAsBuilder => new NpgsqlConnectionStringBuilder(ConnectionString);
        public NpgsqlConnection NpgsqlConnection { get; }
        public DatabaseFixture(string initialConnectionString)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(initialConnectionString)
            {
                Encoding = "UTF8",
                ConnectionPruningInterval = 2,
                ConnectionIdleLifetime = 10,
                Enlist = true
            };
            ConnectionString = connectionStringBuilder.ConnectionString;
            CreateDatabase();
            NpgsqlConnection = new NpgsqlConnection(ConnectionString);
            NpgsqlConnection.Open();
        }
        private string GetConnectionString(Action<NpgsqlConnectionStringBuilder> options)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(ConnectionString);
            options(connectionStringBuilder);
            return connectionStringBuilder.ConnectionString;
        }
        private void CreateDatabase()
        {
            var baseConnection = GetConnectionString(builder => builder.Database = null);
            using (var connection = new NpgsqlConnection(baseConnection))
            {
                connection.Open();
                // Select database to run CREATE DATABASE query on
                connection.ChangeDatabase("postgres");
#pragma warning disable CA2100
                var createDatabaseCommand =
                    new NpgsqlCommand(
                        $"CREATE DATABASE \"{ConnectionStringAsBuilder.Database}\" WITH OWNER = postgres CONNECTION LIMIT = -1;",
                        connection);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                createDatabaseCommand.ExecuteNonQuery();
            }
        }
        public void Dispose()
        {
            NpgsqlConnection?.Dispose();
            DropDatabase();
        }

        private void DropDatabase()
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("postgres"); // Change database, so we can drop the current one.
#pragma warning disable CA2100
                var closeAllConnectionsCommand =
                    new NpgsqlCommand(
                        $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{ConnectionStringAsBuilder.Database}';",
                        connection);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                closeAllConnectionsCommand.ExecuteNonQuery();
#pragma warning disable CA2100
                var dropDatabaseCommand =
                    new NpgsqlCommand(
                        $@"DROP DATABASE IF EXISTS ""{ConnectionStringAsBuilder.Database}""", connection);
#pragma warning restore CA2100
                dropDatabaseCommand.ExecuteNonQuery();
            }
        }
    }
}
public static class TestConnectionStringSource
{
    public static string GenerateConnectionString()
    {
        var builder =
            new NpgsqlConnectionStringBuilder(connectionString:
                Environment.GetEnvironmentVariable("TestConnectionString")
                ?? $"server=localhost;Port=5432;userid=postgres;password=parool")
            {
                Database = "tests-db-" + Guid.NewGuid().ToString().ToLower(),
                Enlist = true
            };
        return builder.ConnectionString;
    }
}