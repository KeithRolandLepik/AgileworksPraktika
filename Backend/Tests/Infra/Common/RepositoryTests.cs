using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Infra.Common
{
    [TestClass]
    public class RepositoryTests : TestDatabaseInitializer
    {
        [TestCleanup]
        public void Cleanup()
        {
            _databaseFixture.Dispose();
        }
    }
}
