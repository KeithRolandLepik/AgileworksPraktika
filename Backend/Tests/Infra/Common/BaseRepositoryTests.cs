using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Marten;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Infra.Common
{
    [TestClass]
    public class BaseRepositoryTests : RepositoryTests
    {
        protected FeedbackData data;
        private TestClass obj;

        private class TestClass : BaseRepository<Feedback, FeedbackData>
        {
            public TestClass(IDocumentSession documentSession) : base(documentSession){}

            protected override FeedbackData copyData(FeedbackData d)
            {
                var x = new FeedbackData
                {
                    Id = d.Id,
                    DueDate = d.DueDate,
                    DateAdded = d.DateAdded,
                    Overdue = d.Overdue,
                    Completed = d.Completed,
                    Description = d.Description
                };
                return x;
            }

            protected override Feedback toDomainObject(FeedbackData d) => new Feedback(d);

        }
        
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            obj = new TestClass(_documentSession);
            data = GetRandom.FeedbackData();
        }
        [TestMethod]
        public void GetTest()
        {
            var count = GetRandom.RndInteger(1, 10);
            var countBefore = obj.Get().GetAwaiter().GetResult().Count;

            for (int i = 0; i < count; i++)
            {
                data = GetRandom.FeedbackData();
                AddTest();
            }

            Assert.AreEqual(count + countBefore, obj.Get().GetAwaiter().GetResult().Count);
        }
        [TestMethod]
        public void DeleteTest()
        {
            AddTest();
            var count = obj.Get().GetAwaiter().GetResult().Count;
            var expected = obj.Get(data.Id).GetAwaiter().GetResult();
            TestArePropertyValuesEqual(expected.Data, data);

            obj.Delete(data.Id).GetAwaiter().GetResult();
            var newCount = obj.Get().GetAwaiter().GetResult().Count;
            expected = obj.Get(data.Id).GetAwaiter().GetResult();

            Assert.IsNull(expected.Data);
            Assert.AreNotEqual(count, newCount);
        }
        [TestMethod]
        public void AddTest()
        {
            var expected = obj.Get(data.Id).GetAwaiter().GetResult();
            Assert.IsNull(expected.Data);
            expected = obj.Add(new Feedback(data)).GetAwaiter().GetResult();
            TestArePropertyValuesEqual(expected.Data, data);
            data = expected.Data;

        }
        [TestMethod]
        public void UpdateTest()
        {
            AddTest();
            var newData = GetRandom.FeedbackData();
            var entityToUpdate = obj.Get(data.Id).GetAwaiter().GetResult();

            entityToUpdate.Data.DueDate = newData.DueDate;
            entityToUpdate.Data.Completed = newData.Completed;
            entityToUpdate.Data.DateAdded = newData.DateAdded;
            entityToUpdate.Data.Description = newData.Description;
            entityToUpdate.Data.Overdue = newData.Overdue;

            obj.Update(entityToUpdate).GetAwaiter().GetResult();
            var expected = obj.Get(entityToUpdate.Data.Id).GetAwaiter().GetResult();

            TestArePropertyValuesEqual(expected.Data, entityToUpdate.Data);
        }
    }
}
