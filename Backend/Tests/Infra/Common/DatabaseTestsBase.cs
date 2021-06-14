using Marten;
using Microsoft.Extensions.DependencyInjection;
using Soft;
using System;
using Tests.Common;
using Microsoft.Extensions.Hosting;
using Weasel.Postgresql;

namespace Tests.Infra.Common
{
    public class DatabaseTestsBase : BaseTests
    {
        internal DatabaseFixture DatabaseFixture;
        internal IServiceProvider Services;
        internal IDocumentSession DocumentSession;

        public void InitializeTestDatabase()
        {
            DatabaseFixture = new DatabaseFixture(
            TestConnectionStringSource.GenerateConnectionString());

            Services = Program.CreateHostBuilder(Array.Empty<string>())
            .ConfigureServices(services => services.AddMarten(options =>
            {
                options.Connection(DatabaseFixture.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.All;
            })).Build().Services;

            DocumentSession = Services.GetRequiredService<IDocumentSession>();
        }
    }
}