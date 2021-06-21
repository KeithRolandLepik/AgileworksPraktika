using Npgsql;
using System;

namespace Tests.Common
{
    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get; private set; }
        internal NpgsqlConnectionStringBuilder ConnectionStringBuilder => 
            new(ConnectionString);
        public NpgsqlConnection NpgsqlConnection { get; }
        public DatabaseFixture(string initialConnectionString)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(initialConnectionString);
            ConnectionString = connectionStringBuilder.ConnectionString;
            CreateDatabase();
            NpgsqlConnection = new NpgsqlConnection(ConnectionString);
            NpgsqlConnection.Open();
        }
        internal string GetConnectionString(Action<NpgsqlConnectionStringBuilder> options)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(ConnectionString);
            options(connectionStringBuilder);
            return connectionStringBuilder.ConnectionString;
        }
        internal void CreateDatabase()
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
                        $"CREATE DATABASE \"{ConnectionStringBuilder.Database}\"",
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

        internal void DropDatabase()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            connection.ChangeDatabase("postgres"); // Change database, so we can drop the current one.
#pragma warning disable CA2100
            var closeAllConnectionsCommand =
                new NpgsqlCommand(
                    $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = " +
                    $"'{ConnectionStringBuilder.Database}';",
                    connection);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
            closeAllConnectionsCommand.ExecuteNonQuery();
#pragma warning disable CA2100
            var dropDatabaseCommand =
                new NpgsqlCommand(
                    $@"DROP DATABASE IF EXISTS ""{ConnectionStringBuilder.Database}""", connection);
#pragma warning restore CA2100
            dropDatabaseCommand.ExecuteNonQuery();
        }
    }
}
public static class TestConnectionStringSource
{
    public static string GenerateConnectionString()
    {
        var builder =
            new NpgsqlConnectionStringBuilder
                (connectionString: $"server=localhost;Port=5432;userid=postgres;password=parool")
            {
                Database = "tests-db-" + Guid.NewGuid().ToString().ToLower()
            };
        return builder.ConnectionString;
    }
}