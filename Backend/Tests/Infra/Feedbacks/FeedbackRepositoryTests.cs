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
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new FeedbackRepository(_documentSession);
        }
    }
}
