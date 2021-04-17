using Data.Feedbacks;
using Domain.Common;
using Domain.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Domain.Feedbacks
{
    [TestClass]
    public class FeedbackTests : BaseClassTests<Feedback, Entity<FeedbackData>>
    {
        [TestMethod]
        public void CheckOverDueTest()
        {
            var d = new FeedbackData { Id = 1, Description = "asd", Completed = false, Overdue = false, DateAdded = DateTime.Now, DueDate = DateTime.Now.AddHours(50)};
            obj = new Feedback(d);
            Assert.IsFalse(obj.Data.Overdue);
            d.DueDate = DateTime.Now;
            obj = new Feedback(d);
            Assert.IsTrue(obj.Data.Overdue);
        }
    }
}
