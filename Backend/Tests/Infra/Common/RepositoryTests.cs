using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Soft;
using System;
using Tests.Common;
using Microsoft.Extensions.Hosting;
using Weasel.Postgresql;

namespace Tests.Infra.Common
{
    [TestClass]
    public class RepositoryTests : BaseTests
    {
        internal DatabaseFixture _databaseFixture;
        internal IServiceProvider _services;
        internal IDocumentSession _documentSession;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _databaseFixture = new DatabaseFixture(
            TestConnectionStringSource.GenerateConnectionString());

            _services = Program.CreateHostBuilder(Array.Empty<string>())
            .ConfigureServices(services => services.AddMarten(options =>
            {
                options.Connection(_databaseFixture.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.All;
            })).Build().Services;

            _documentSession = _services.GetRequiredService<IDocumentSession>();
        }
        [TestCleanup]
        public void Cleanup()
        {
            _databaseFixture.Dispose();
        }
    }
}
