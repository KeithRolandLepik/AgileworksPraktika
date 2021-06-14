using Data.Feedbacks;
using Domain.Feedbacks;
using Facade.Feedbacks;
using Infra.Common;
using Marten;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Infra.Common
{
    [TestClass]
    public class BaseRepositoryTests : RepositoryTests
    {
        protected FeedbackData EntityData;
        internal TestClass Object;

        internal class TestClass : BaseRepository<Feedback, FeedbackData>
        {
            public TestClass(IDocumentSession documentSession) : base(documentSession) { }

            protected override FeedbackData copyData(FeedbackData entityData)
            {
                return
                    new FeedbackData
                    {
                        Id = entityData.Id,
                        DueDate = entityData.DueDate,
                        Description = entityData.Description,
                        DateAdded = entityData.DateAdded,
                        Completed = entityData.Completed,
                        Overdue = entityData.Overdue
                    };
            }

            protected override Feedback toDomainObject(FeedbackData entityData) => new Feedback(entityData);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();
            Object = new TestClass(DocumentSession);
            EntityData = GetRandom.FeedbackData();
        }

        [TestMethod]
        public void Get_should_retrieve_all_feedbacks_from_database()
        {
            var count = GetRandom.RndInteger(1, 10);
            
            // Act
            var initialCount = Object.Get().GetAwaiter().GetResult().Count;
            for (int i = 0; i < count; i++)
            {
                EntityData = GetRandom.FeedbackData();
                Add_should_store_a_feedback_in_database();
            }
            var finalCount = Object.Get().GetAwaiter().GetResult().Count;

            // Assert
            Assert.AreEqual(count + initialCount, finalCount);
        }

        [TestMethod]
        public void Delete_should_remove_entity_from_database()
        {
            // Act
            Add_should_store_a_feedback_in_database();
            var count = Object.Get().GetAwaiter().GetResult().Count;
            var feedbackToBeDeleted = Object.Get(EntityData.Id).GetAwaiter().GetResult();
            TestArePropertyValuesEqual(feedbackToBeDeleted.Data, EntityData);
            Object.Delete(EntityData.Id).GetAwaiter().GetResult();
            var newCount = Object.Get().GetAwaiter().GetResult().Count;
            feedbackToBeDeleted = Object.Get(EntityData.Id).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(feedbackToBeDeleted.Data);
            Assert.AreNotEqual(count, newCount);
        }

        [TestMethod]
        public void Add_should_store_a_feedback_in_database()
        {
            var feedbackInput = new FeedbackInput()
            {
                Description = GetRandom.RndInteger(1,1000).ToString(),
                DueDate = GetRandom.Datetime(),
            };
            var entityToBeAdded = FeedbackMapper.MapToDomainFromInput(feedbackInput);
            EntityData = entityToBeAdded.Data;

            // Act
            var initialDatabaseFeedback = Object.Get(EntityData.Id).GetAwaiter().GetResult();
            var addedFeedback = Object.Add(entityToBeAdded).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(initialDatabaseFeedback.Data);
            TestArePropertyValuesEqual(addedFeedback.Data, EntityData);

            EntityData = addedFeedback.Data;
        }

        [TestMethod]
        public void UpdateTest()
        {
            Add_should_store_a_feedback_in_database();
            var newFeedbackUpdate = new FeedbackUpdate
                {
                    Completed = true,
                    Description = GetRandom.RndInteger(1,100).ToString(),
                    DueDate = GetRandom.Datetime(),
                };

            // Act
            var initialFeedbackData = Object.Get(EntityData.Id).GetAwaiter().GetResult().Data;
            var feedbackToUpdate = FeedbackMapper.MapToDomainFromUpdate(new Feedback(initialFeedbackData), newFeedbackUpdate);
            //Object.Update(feedbackToUpdate).GetAwaiter().GetResult();
            var updatedFeedback = Object.Get(initialFeedbackData.Id).GetAwaiter().GetResult();

            // Assert
            TestArePropertyValuesEqual(updatedFeedback.Data, feedbackToUpdate.Data);
        }

    }
}
