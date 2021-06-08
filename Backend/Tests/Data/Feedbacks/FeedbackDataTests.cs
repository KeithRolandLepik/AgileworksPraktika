using Data.Common;
using Data.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Data.Feedbacks
{
    [TestClass]
    public class FeedbackDataTests : BaseClassTests<FeedbackData, DefinedEntityData>
    {
        private class TestClass : FeedbackData { }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new TestClass();
        }
        [TestMethod]
        public void DueDateTest()
        {
            var val = GetRandom.Datetime();

            Assert.AreNotEqual(obj.DueDate, val);
            obj.DueDate = val;
            Assert.AreEqual(obj.DueDate, val);
        }
        [TestMethod]
        public void DateAddedTest()
        {
            var val = GetRandom.Datetime();
            Assert.AreNotEqual(obj.DateAdded, val);
            obj.DateAdded = val;
            Assert.AreEqual(obj.DateAdded, val);
        }
        [TestMethod]
        public void CompletedTest()
        {
            Assert.AreEqual(obj.Completed, false);
            obj.Completed = true;
            Assert.AreEqual(obj.Completed, true);
        }
        [TestMethod]
        public void OverdueTest()
        {
            var val = GetRandom.Bool();
            Assert.AreNotEqual(obj.Overdue, val);
            obj.Overdue = val;
            Assert.AreEqual(obj.Overdue, val);
        }
    }
}
