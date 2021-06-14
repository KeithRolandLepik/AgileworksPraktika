using System;
using AutoFixture;
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
        internal TestClass Sut;

        internal class TestClass : BaseRepository<Feedback, FeedbackData>
        {
            public TestClass(IDocumentStore store) : base(store) { }

            protected override Feedback ToDomainObject(FeedbackData entityData) => new Feedback(entityData);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
            InitializeTestDatabase();
            Sut = new TestClass(DocumentStore);
            EntityData = Fixture.Create<FeedbackData>();
        }

        [TestMethod]
        public void Get_should_retrieve_all_feedbacks_from_database()
        {
            var count = 20;
            PopulateDatabase(count);

            // Act
            var initialCount = Sut.Get().GetAwaiter().GetResult().Count;
            PopulateDatabase(count);
            var finalCount = Sut.Get().GetAwaiter().GetResult().Count;

            // Assert
            Assert.AreEqual(count + initialCount, finalCount);
        }

        [TestMethod]
        public void Delete_should_remove_entity_from_database()
        {
            PopulateDatabase(20);
            // Act
            var count = Sut.Get().GetAwaiter().GetResult().Count;
            var feedbackToBeDeleted= Sut.Get().GetAwaiter().GetResult()[0];
            Sut.Delete(feedbackToBeDeleted.Data.Id).GetAwaiter().GetResult();
            var newCount = Sut.Get().GetAwaiter().GetResult().Count;
            feedbackToBeDeleted = Sut.Get(feedbackToBeDeleted.Data.Id).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(feedbackToBeDeleted.Data);
            Assert.AreEqual(count-newCount, 1);
        }

        [TestMethod]
        public void Add_should_store_a_feedback_in_database()
        {
            var feedbackInput = new AddFeedbackRequest
            {
                Description = Fixture.Create<string>(),
                DueDate = Fixture.Create<DateTime>(),
            };
            var entityToBeAdded = FeedbackMapper.MapToDomainFromAddRequest(feedbackInput);
            
            // Act
            var initialDatabaseFeedback = Sut.Get(entityToBeAdded.Data.Id).GetAwaiter().GetResult();
            var addedFeedback = Sut.Add(entityToBeAdded).GetAwaiter().GetResult();
            
            // Assert
            Assert.IsNull(initialDatabaseFeedback.Data);
            AssertArePropertyValuesEqual(addedFeedback.Data, entityToBeAdded.Data);

            EntityData = addedFeedback.Data;
        }

        [TestMethod]
        public void Update_should_update_existing_database_item()
        {
            PopulateDatabase(20);
            
            var newFeedbackUpdate = new UpdateFeedbackRequest
                {
                    IsCompleted = true,
                    Description = Fixture.Create<string>(),
                    DueDate = Fixture.Create<DateTime>()
            };

            // Act
            var initialFeedbackData = Sut.Get().GetAwaiter().GetResult()[0];
            var feedbackToUpdate = FeedbackMapper.MapToDomainFromUpdateRequest(initialFeedbackData, newFeedbackUpdate);
            Sut.Update(feedbackToUpdate).GetAwaiter().GetResult();

            // Assert
            var initialFeedbackDataCopy2 = Sut.Get(initialFeedbackData.Data.Id).GetAwaiter().GetResult();
            initialFeedbackDataCopy2.Data.IsOverdue = feedbackToUpdate.Data.IsOverdue;
            AssertArePropertyValuesEqual(initialFeedbackDataCopy2.Data, feedbackToUpdate.Data);
        }

        public void PopulateDatabase(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var feedbackInput = new AddFeedbackRequest
                {
                    Description = Fixture.Create<string>(),
                    DueDate = Fixture.Create<DateTime>()
                };
                var entityToBeAdded = FeedbackMapper.MapToDomainFromAddRequest(feedbackInput);
                
                Sut.Add(entityToBeAdded).GetAwaiter().GetResult();
            }
        }
    }
}
