using Facade.Common;
using Facade.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Feedbacks
{
    [TestClass]
    public class FeedbackViewTests : BaseClassTests<FeedbackModel, DefinedEntityModel>
    {
        private class TestClass : FeedbackModel { }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            Object = new TestClass();
            
        }
        [TestMethod]
        public void DueDate_should_be_gettable_and_settable()
        {
            var randomValue = GetRandom.Datetime();

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
            var randomValue = GetRandom.Datetime();

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
        public void Overdue_should_be_gettable_and_settable()
        {
            var randomValue = GetRandom.Bool();

            // Act
            var initialOverdueValue = Object.IsOverdue;
            Object.IsOverdue = randomValue;

            // Assert
            Assert.AreNotEqual(initialOverdueValue, randomValue);
            Assert.AreEqual(Object.IsOverdue, randomValue);
        }
    }
}

