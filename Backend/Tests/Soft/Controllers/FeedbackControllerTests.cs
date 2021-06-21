using System;
using AutoFixture;
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
            Repository = new FeedbackRepository(DocumentStore);
            Controller = new FeedbackController(Repository);
            Fixture = new Fixture();
            Count = 20;
            AddFeedbacks();
        }

        [TestMethod]
        public void GetFeedbacks_should_return_all_feedbacks()
        {
            // Act
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(results.Value.Count, Repository.Get().GetAwaiter().GetResult().Count);
        }

        [TestMethod]
        public void GetFeedback_should_return_a_feedback_and_response_code()
        {
            var id = Fixture.Create<int>();

            // Act
            var notFoundResult = Controller.GetFeedback(id).GetAwaiter().GetResult();
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            id = results.Value[0].Id;
            var okObjectResult= Controller.GetFeedback(id).GetAwaiter().GetResult();
            
            // Assert
            Assert.AreEqual(notFoundResult.Result.GetType(), typeof(NotFoundResult));
            Assert.AreEqual(okObjectResult.Result.GetType(), typeof(OkObjectResult));

        }

        [TestMethod]
        public void PutFeedback_should_return_a_okResult_if_successful_or_badRequest_if_unsuccessful()
        {
            var id = Fixture.Create<int>();
            var updateData1 = new UpdateFeedbackRequest
            {
                IsCompleted = true,
                Description = "newTest1234141411",
                DueDate = Fixture.Create<DateTime>()
            };
            var updateData2 = new UpdateFeedbackRequest
            {
                DueDate = Fixture.Create<DateTime>()
            };
            var updateData3 = new UpdateFeedbackRequest
            {
                Description = "newTest25151234351431234"
            };
            var updateData4 = new UpdateFeedbackRequest
            {
                IsCompleted = false
            };

            // Act
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            var putResult1 = Controller.PutFeedback(id, updateData1).GetAwaiter().GetResult();
            id = results.Value[0].Id;
            var putResult2 = Controller.PutFeedback(id, updateData2).GetAwaiter().GetResult();
            var putResult3 = Controller.PutFeedback(id, updateData3).GetAwaiter().GetResult();
            var putResult4 = Controller.PutFeedback(id, updateData4).GetAwaiter().GetResult();
            var putResult5 = Controller.PutFeedback(id, updateData1).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(id);
            Assert.AreEqual(putResult1.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(putResult2.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(putResult3.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(putResult4.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(putResult5.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void PostFeedback_should_return_createdAtActionResult_if_successful_or_badRequestResult_if_unSuccesful()
        {
            var inputData1 = new AddFeedbackRequest { 
                Description = Fixture.Create<string>(),
                DueDate = Fixture.Create<DateTime>()
            };
            var inputData2 = new AddFeedbackRequest
            {
                Description = Fixture.Create<string>()
            };
            var inputData3 = new AddFeedbackRequest
            {
                DueDate = Fixture.Create<DateTime>()
            };

            // Act
            var result1 = Controller.PostFeedback(inputData1).GetAwaiter().GetResult();
            var result2 = Controller.PostFeedback(inputData2).GetAwaiter().GetResult();
            var result3 = Controller.PostFeedback(inputData3).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result1.Result.GetType(), typeof(CreatedAtActionResult));
            Assert.AreEqual(result2.Result.GetType(), typeof(BadRequestResult));
            Assert.AreEqual(result3.Result.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_noContentResult_if_successful_or_notFoundResult_if_unSuccessful()
        {
            var results = Controller.GetFeedbacks().GetAwaiter().GetResult();
            var countBefore = results.Value.Count;
            var id = results.Value[0].Id;

            // Act
            var deleteResult = Controller.DeleteFeedback(Fixture.Create<int>()).GetAwaiter().GetResult();
            var deleteResult2 = Controller.DeleteFeedback(id).GetAwaiter().GetResult();
            var countAfter = Controller.GetFeedbacks().GetAwaiter().GetResult().Value.Count;
            
            // Assert
            Assert.AreEqual(deleteResult.GetType(), typeof(NotFoundResult));
            Assert.AreEqual(deleteResult2.GetType(), typeof(NoContentResult));
            Assert.AreEqual(countBefore - 1, countAfter);
        }

        public void AddFeedbacks()
        {
            for (var i = 0; i < Count; i++)
            {
                var inputData = new AddFeedbackRequest { Description = Fixture.Create<string>(), DueDate = Fixture.Create<DateTime>() };
                var l = Repository.Add(FeedbackMapper.MapToDomainFromAddRequest(inputData)).GetAwaiter().GetResult();
            }
        }
    }
}
