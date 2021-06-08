using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Infra.Feedbacks;
using Marten;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Infra.Common
{
    [TestClass]
    public class BaseRepositoryTests : BaseTests
    {
        protected FeedbackData data;

        //private IServiceScope _serviceScope;
        //protected IServiceProvider ServiceProvider
        //=> _serviceScope.ServiceProvider;
        //protected IDocumentSession _documentSession => ServiceProvider.GetRequiredService<IDocumentSession>();
        
        internal class TestClass : BaseRepository<Feedback, FeedbackData>
        {
            public TestClass(IDocumentSession documentSession) : base(documentSession)
            {
            }

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

        private IDocumentSession _documentSession;
        internal TestClass obj;

        [TestInitialize]
        public void TestInitialize(IDocumentSession documentSession)
        {
            _documentSession = documentSession;

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
