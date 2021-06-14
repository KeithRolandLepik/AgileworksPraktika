using System;
using AutoFixture;
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
            Object = new TestClass();
            Fixture = new Fixture();
        }

        [TestMethod]
        public void DueDate_should_be_gettable_and_settable()
        {
            var randomValue = Fixture.Create<DateTime>();

            // Act
            var initialDueDateValue = Object.DueDate;
            Object.DueDate = randomValue;

            // Assert
            Assert.AreNotEqual(initialDueDateValue, randomValue);
            Assert.AreEqual(Object.DueDate, randomValue);

        }

        [TestMethod]
        public void DateAdded_should_be_gettable_and_settable()
        {
            var randomValue = Fixture.Create<DateTime>();

            // Act
            var initialDateAddedValue = Object.DateAdded;
            Object.DateAdded = randomValue;

            // Assert
            Assert.AreNotEqual(initialDateAddedValue, randomValue);
            Assert.AreEqual(Object.DateAdded, randomValue);
        }

        [TestMethod]
        public void Completed_should_be_gettable_and_settable()
        {
            // Act
            var initialCompletedValue = Object.IsCompleted;
            Object.IsCompleted = true;

            // Assert
            Assert.AreEqual(initialCompletedValue, false);
            Assert.AreEqual(Object.IsCompleted, true);
        }

        [TestMethod]
        public void Overdue_should_be_gettable_and_should_calculate_value()
        {
            // Act
            Object.DueDate = DateTime.Now;
            var overDue = Object.IsOverdue;
            Object.DueDate = DateTime.Now.AddDays(599);
            var notOverDue = Object.IsOverdue;

            // Assert
            Assert.IsTrue(overDue);
            Assert.IsFalse(notOverDue);
        }
    }
}
