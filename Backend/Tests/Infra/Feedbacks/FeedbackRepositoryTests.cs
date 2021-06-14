using Data.Feedbacks;
using Infra.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Infra.Common;

namespace Tests.Infra.Feedbacks
{
    [TestClass]
    public class FeedbackRepositoryTests : RepositoryTests
    {

        protected FeedbackData EntityData;
        protected FeedbackRepository TestRepository;
        protected int Count;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();
            TestRepository = new FeedbackRepository(DocumentStore);
        }
    }
}
