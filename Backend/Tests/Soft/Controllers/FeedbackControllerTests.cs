using Microsoft.VisualStudio.TestTools.UnitTesting;
using Soft.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infra.Feedbacks;
using Facade.Feedbacks;
using Tests.Infra.Common;

namespace Tests.Soft.Controllers
{
    [TestClass]
    public class FeedbackControllerTests : DatabaseTestsBase
    {
        protected FeedbackRepository Repository;
        protected FeedbackController Controller;
        protected int Count;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTestDatabase();

            Repository = new FeedbackRepository(DocumentSession);
            Controller = new FeedbackController(Repository);

            Count = GetRandom.RndInteger(5, 10);
            AddFeedbacks();
        }

        [TestMethod]
        public void GetFeedbacksTest()
        {
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();

            Assert.AreEqual(results.Value.Count, Repository.Get().GetAwaiter().GetResult().Count);
        }

        [TestMethod]
        public void GetFeedbackTest()
        {
            var id = GetRandom.RndInteger(500, 1000);
            var result = Controller.GetFeedback(id).GetAwaiter().GetResult();
            Assert.AreEqual(result.Result.GetType(), typeof(NotFoundResult));

            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            id = results.Value[0].Id;
            result = Controller.GetFeedback(id).GetAwaiter().GetResult();
            Assert.AreEqual(result.Result.GetType(), typeof(OkObjectResult));
        }

        [TestMethod]
        public void PutFeedbackTest()
        {
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            var id = GetRandom.RndInteger(500, 1000);
            var updateData1 = new FeedbackUpdate
            {
                Completed = true,
                Description = "newTest",
                DueDate = GetRandom.Datetime()
            };
            var put = Controller.PutFeedback(id, updateData1).GetAwaiter().GetResult();
            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


            id = results.Value[0].Id;
            Assert.IsNotNull(id);
            var updateData2 = new FeedbackUpdate
            {
                DueDate = GetRandom.Datetime()
            };
            put = Controller.PutFeedback(id, updateData2).GetAwaiter().GetResult();
            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


            var updateData3 = new FeedbackUpdate
            {
                Description = "newTest"
            };
            put = Controller.PutFeedback(id, updateData3).GetAwaiter().GetResult();
            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


            var updateData4 = new FeedbackUpdate
            {
                Completed = false
            };
            put = Controller.PutFeedback(id, updateData4).GetAwaiter().GetResult();
            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


            put = Controller.PutFeedback(id, updateData1).GetAwaiter().GetResult();
            Assert.AreEqual(put.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void PostFeedbackTest()
        {
            //Getrandom tagastab 1995-st alates random date, aga uuel inputil peab olema date praegusest suurem

            var inputData = new FeedbackInput { Description = "test", DueDate = GetRandom.Datetime() };
            var result = Controller.PostFeedback(inputData).GetAwaiter().GetResult();
            Assert.AreEqual(result.Result.GetType(), typeof(CreatedAtActionResult));

            var inputData2 = new FeedbackInput { Description = "test2" };
            result = Controller.PostFeedback(inputData2).GetAwaiter().GetResult();
            Assert.AreEqual(result.Result.GetType(), typeof(BadRequestResult));

            var inputData3 = new FeedbackInput { DueDate = GetRandom.Datetime() };
            result = Controller.PostFeedback(inputData3).GetAwaiter().GetResult();
            Assert.AreEqual(result.Result.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteFeedbackTest()
        {
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            var countBefore = results.Value.Count;

            var deleteResult = Controller.DeleteFeedback(GetRandom.RndInteger(500, 1000)).GetAwaiter().GetResult();
            Assert.AreEqual(deleteResult.GetType(), typeof(NotFoundResult));

            var id = results.Value[0].Id;
            deleteResult = Controller.DeleteFeedback(id).GetAwaiter().GetResult();

            var countAfter = Controller.GetFeedbacks().GetAwaiter().GetResult().Value.Count;
            Assert.AreEqual(deleteResult.GetType(), typeof(NoContentResult));
            Assert.AreEqual(countBefore - 1, countAfter);
        }

        public void AddFeedbacks()
        {
            for (int i = 0; i < Count; i++)
            {
                var inputData = new FeedbackInput { Description = "test" + i.ToString(), DueDate = GetRandom.Datetime() };
                var l = Repository.Add(FeedbackMapper.MapToDomainFromInput(inputData)).GetAwaiter().GetResult();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            DatabaseFixture.Dispose();
        }
    }
}
