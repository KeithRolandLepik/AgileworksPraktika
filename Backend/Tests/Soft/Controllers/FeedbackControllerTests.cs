//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Soft.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Data.Feedbacks;
//using Infra.Feedbacks;
//using Microsoft.EntityFrameworkCore;
//using Facade.Feedbacks;
//using Domain.Feedbacks;
//using Tests.Infra.Common;

//namespace Tests.Soft.Controllers
//{
//    [TestClass]
//    public class FeedbackControllerTests : RepositoryTests
//    {
//        protected IFeedbackRepository repo;
//        protected FeedbackController obj;
//        protected int count;

//        [TestInitialize]
//        public override void TestInitialize()
//        {
//            base.TestInitialize();

//            repo = new FeedbackRepository(_documentSession);
//            obj = new FeedbackController(repo);

//            count = GetRandom.RndInteger(5, 10);
//            AddFeedbacks();
//        }

//        [TestMethod]
//        public void GetFeedbacksTest()
//        {
//            var results = obj.GetFeedbacks().GetAwaiter().GetResult();

//            Assert.AreEqual(results.Value.Count, repo.Get().GetAwaiter().GetResult().Count);
//        }

//        [TestMethod]
//        public void GetFeedbackTest()
//        {
//            var id = GetRandom.RndInteger(500, 1000);
//            var result = obj.GetFeedback(id).GetAwaiter().GetResult();
//            Assert.AreEqual(result.Result.GetType(), typeof(NotFoundResult));

//            var results = obj.GetFeedbacks().GetAwaiter().GetResult();
//            id = results.Value[0].Id;
//            result = obj.GetFeedback(id).GetAwaiter().GetResult();
//            Assert.AreEqual(result.Result.GetType(), typeof(OkObjectResult));
//        }

//        [TestMethod]
//        public void PutFeedbackTest()
//        {
//            var results = obj.GetFeedbacks().GetAwaiter().GetResult();
//            var id = GetRandom.RndInteger(500, 1000);
//            var updateData1 = new FeedbackUpdate
//            {
//                Completed = true,
//                Description = "newTest",
//                DueDate = GetRandom.Datetime()
//            };
//            var put = obj.PutFeedback(id, updateData1).GetAwaiter().GetResult();
//            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


//            id = results.Value[0].Id;
//            Assert.IsNotNull(id);
//            var updateData2 = new FeedbackUpdate
//            {
//                DueDate = GetRandom.Datetime()
//            };
//            put = obj.PutFeedback(id, updateData2).GetAwaiter().GetResult();
//            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


//            var updateData3 = new FeedbackUpdate
//            {
//                Description = "newTest"
//            };
//            put = obj.PutFeedback(id, updateData3).GetAwaiter().GetResult();
//            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


//            var updateData4 = new FeedbackUpdate
//            {
//                Completed = false
//            };
//            put = obj.PutFeedback(id, updateData4).GetAwaiter().GetResult();
//            Assert.AreEqual(put.GetType(), typeof(BadRequestResult));


//            put = obj.PutFeedback(id, updateData1).GetAwaiter().GetResult();
//            Assert.AreEqual(put.GetType(), typeof(OkResult));
//        }

//        [TestMethod]
//        public void PostFeedbackTest()
//        {
//            //Getrandom tagastab 1995-st alates random date, aga uuel inputil peab olema date praegusets suurem

//            var inputData = new FeedbackInput { Description = "test", DueDate = GetRandom.Datetime() };
//            var result = obj.PostFeedback(inputData).GetAwaiter().GetResult();
//            Assert.AreEqual(result.Result.GetType(), typeof(CreatedAtActionResult));

//            var inputData2 = new FeedbackInput { Description = "test2" };
//            result = obj.PostFeedback(inputData2).GetAwaiter().GetResult();
//            Assert.AreEqual(result.Result.GetType(), typeof(BadRequestResult));

//            var inputData3 = new FeedbackInput { DueDate = GetRandom.Datetime() };
//            result = obj.PostFeedback(inputData3).GetAwaiter().GetResult();
//            Assert.AreEqual(result.Result.GetType(), typeof(BadRequestResult));
//        }

//        [TestMethod]
//        public void DeleteFeedbackTest()
//        {
//            var results = obj.GetFeedbacks().GetAwaiter().GetResult();
//            var countBefore = results.Value.Count;

//            var deleteResult = obj.DeleteFeedback(GetRandom.RndInteger(500, 1000)).GetAwaiter().GetResult();
//            Assert.AreEqual(deleteResult.GetType(), typeof(NotFoundResult));

//            var id = results.Value[0].Id;
//            deleteResult = obj.DeleteFeedback(id).GetAwaiter().GetResult();

//            var countAfter = obj.GetFeedbacks().GetAwaiter().GetResult().Value.Count;
//            Assert.AreEqual(deleteResult.GetType(), typeof(NoContentResult));
//            Assert.AreEqual(countBefore - 1, countAfter);
//        }

//        public void AddFeedbacks()
//        {
//            for (int i = 0; i < count; i++)
//            {
//                var inputData = new FeedbackInput { Description = "test" + i.ToString(), DueDate = GetRandom.Datetime() };
//                obj.PostFeedback(inputData).GetAwaiter();
//            }
//        }
//    }
//}
