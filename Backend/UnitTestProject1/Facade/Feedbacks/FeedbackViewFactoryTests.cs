using Domain.Feedbacks;
using Facade.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Feedbacks
{
    [TestClass]
    public class FeedbackViewFactoryTests : BaseTests
    {
        protected GetRandom GetRandom = new GetRandom();

        [TestMethod]
        public void CreateObjectTest()
        {
            var view = new FeedbackView 
            { Overdue = GetRandom.Bool(), Completed = GetRandom.Bool(),
              DateAdded = GetRandom.Datetime(), Description = "test", DueDate = GetRandom.Datetime() };
            var data = FeedbackViewFactory.Create(view).Data;

            TestArePropertyValuesEqual(view, data);
        }
        [TestMethod]
        public void CreateviewTests()
        {
            var data = GetRandom.FeedbackData();

            var view = FeedbackViewFactory.Create(new Feedback(data));

            TestArePropertyValuesEqual(view, data);
        }
    }
}
