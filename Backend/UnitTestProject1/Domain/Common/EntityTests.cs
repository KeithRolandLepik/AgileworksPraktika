using Data.Feedbacks;
using Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Domain.Common
{
    [TestClass]
    public class EntityTests : BaseClassTests<Entity<FeedbackData>, object>
    {
        private class testClass : Entity<FeedbackData>
        {
            public testClass(FeedbackData d = null) : base(d) { }
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new testClass();
        }

        [TestMethod]
        public void IsAbstract()
        {
            Assert.IsTrue(type.IsAbstract);
        }

        [TestMethod]
        public void DataTest()
        {
            var d = new FeedbackData{ Id = 1, Description = "asd", Completed = false, Overdue = false, DateAdded = DateTime.Now, DueDate = new DateTime(2021, 6, 6, 20, 40, 0) };

            Assert.AreNotSame(d, obj.Data);
            obj = new testClass(d);
            Assert.AreSame(d, obj.Data);
        }

        [TestMethod]
        public void DataIsNullTest()
        {
            var d = new FeedbackData { Id = 1, Description = "asd", Completed = false, Overdue = false, DateAdded = DateTime.Now, DueDate = new DateTime(2021, 6, 6, 20, 40, 0) };

            Assert.IsNull(obj.Data);
            obj.Data = d;
            Assert.AreSame(d, obj.Data);
        }

        [TestMethod]
        public void CanSetNullDataTest()
        {
            obj.Data = null;
            Assert.IsNull(obj.Data);
        }
    }
}
