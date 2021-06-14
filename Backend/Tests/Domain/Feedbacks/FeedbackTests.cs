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
        public void Feedback_on_creation_should_calculate_and_set_overdue_value()
        {
            var entityData = new FeedbackData { Id = 1, Description = "asd", IsCompleted = false, IsOverdue = false, DateAdded = DateTime.Now, DueDate = DateTime.Now.AddHours(50) };

            // Act
            Object = new Feedback(entityData);
            var overDueBefore = Object.Data.IsOverdue;
            entityData.DueDate = DateTime.Now.AddHours(-50);
            Object = new Feedback(entityData);

            // Assert
            Assert.IsFalse(overDueBefore);
            Assert.IsTrue(Object.Data.IsOverdue);
        }
    }
}
