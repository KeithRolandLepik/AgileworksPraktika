﻿using Facade.Common;
using Facade.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Feedbacks
{
    [TestClass]
    public class FeedbackViewTests : BaseClassTests<FeedbackView, DefinedEntityView>
    {
        private class TestClass : FeedbackView { }
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
            var val = GetRandom.Bool();
            Assert.AreNotEqual(obj.Completed, val);
            obj.Completed = val;
            Assert.AreEqual(obj.Completed, val);
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
