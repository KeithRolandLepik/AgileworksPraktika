using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Infra.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Infra.Common
{
    [TestClass]
    public class BaseRepositoryTests : BaseClassTests<BaseRepository<Feedback, FeedbackData>, object>
    {
        protected FeedbackData data;
        protected GetRandom GetRandom;
        private class TestClass : BaseRepository<Feedback, FeedbackData>
        {
            public TestClass(DbContext c, DbSet<FeedbackData> s) : base(c, s)
            {

            }

            protected override FeedbackData copyData(FeedbackData d)
            {
                var x = getDataById(d);

                if (x is null) return d;

                x.Id = d.Id;
                x.DueDate = d.DueDate;
                x.DateAdded = d.DateAdded;
                x.Overdue = d.Overdue;
                x.Completed = d.Completed;
                x.Description = d.Description;

                return x;
            }

            protected override Feedback toDomainObject(FeedbackData d) => new Feedback(d);

            protected override Feedback unspecifiedEntity() => new Feedback();
        }
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            var options = new DbContextOptionsBuilder<FeedbackDbContext>().UseInMemoryDatabase("TestDb").Options;
            var c = new FeedbackDbContext(options);
            obj = new TestClass(c, c.FeedbackDatas);
            GetRandom = new GetRandom();
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

            obj.Delete(data.Id).GetAwaiter();
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

        }
        [TestMethod]
        public void UpdateTest()
        {
            var newData = GetRandom.FeedbackData();
            AddTest();
            newData.Id = data.Id;
            newData.DateAdded = newData.DateAdded.Date;
            obj.Update(new Feedback(newData)).GetAwaiter();
            var expected = obj.Get(data.Id).GetAwaiter().GetResult();
            expected.Data.DateAdded = expected.Data.DateAdded.Date; 
            TestArePropertyValuesEqual(expected.Data, newData);

        }
    }
}
