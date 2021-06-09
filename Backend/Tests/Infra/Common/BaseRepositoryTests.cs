using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Soft;
using Tests.Common;
using Microsoft.Extensions.Hosting;
using Weasel.Postgresql;

namespace Tests.Infra.Common
{
    [TestClass]
    public class BaseRepositoryTests : BaseTests
    {
        protected FeedbackData data;
        
        
        private IDocumentSession _documentSession;
        //private IDocumentSession _documentSession => _services.GetRequiredService<IDocumentSession>();
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
        
        internal TestClass obj;
        private DatabaseFixture _databaseFixture;

        private IServiceProvider _services;
        [TestInitialize]
        public void TestInitialize()
        {
            _databaseFixture = new DatabaseFixture(
            TestConnectionStringSource.GenerateConnectionString());

            _services = Program.CreateHostBuilder(Array.Empty<string>())
            .ConfigureServices(services => services.AddMarten(options =>
            {
                options.Connection(_databaseFixture.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.All;
            })).Build().Services;

            _documentSession = _services.GetRequiredService<IDocumentSession>();

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
