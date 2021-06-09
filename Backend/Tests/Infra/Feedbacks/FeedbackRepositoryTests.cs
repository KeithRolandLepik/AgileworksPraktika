using Data.Feedbacks;
using Infra.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Infra.Common;

namespace Tests.Infra.Feedbacks
{
    [TestClass]
    public class FeedbackRepositoryTests : RepositoryTests
    {

        protected FeedbackData data;
        protected FeedbackRepository obj;
        protected int count;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();
            obj = new FeedbackRepository(_documentSession);
        }
    }
}
