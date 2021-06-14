using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Infra.Common
{
    [TestClass]
    public class RepositoryTests : DatabaseTestsBase
    {
        [TestCleanup]
        public void Cleanup()
        {
            DatabaseFixture.Dispose();
        }
    }
}
