using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Feedbacks;
using Facade.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Soft.Controllers;
using FeedbackData = Data.Feedbacks.FeedbackData;

namespace Tests.Soft.Controllers
{
    [TestClass]
    public class MockFeedbackControllerTests : BaseTests
    {
        internal FeedbackController Controller;
        internal Mock<IFeedbackRepository> MockRepository;
        internal FeedbackRepositoryMock FeedbackMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
            MockRepository = new Mock<IFeedbackRepository>();
            FeedbackMock = new FeedbackRepositoryMock();
        }

        [TestMethod]
        public void GetFeedbacks_should_return_all_feedbacks()
        {
            var l = new List<Feedback>();
            for (int i = 0; i < 5; i++)
            {
                l.Add(new Feedback(Fixture.Create<FeedbackData>()));
            }
            FeedbackMock.SetupFeedbacks(l);
            //MockRepository.Setup(x => x.Get().Result).Returns(l);
            Controller = new FeedbackController(FeedbackMock);

            // Act
            var result = Controller.GetFeedbacks().GetAwaiter().GetResult().Value;

            // Assert
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void GetFeedback_should_return_feedback_with_given_id()
        {
            var data = Fixture.Create<FeedbackData>();
            var entity = new Feedback(data);

            //MockRepository.DefaultValue = DefaultValue.Mock;
            //MockRepository.Setup(x =>
            //x.Get(data.Id)).Returns(Task.FromResult(entity));
            
            FeedbackMock.SetupUserFeedback(entity);
            Controller = new FeedbackController(FeedbackMock);

            // Act
            var result = ((FeedbackModel) ((OkObjectResult)
                Controller.GetFeedback(data.Id).GetAwaiter().GetResult().Result).Value);

            // Assert
            AssertArePropertyValuesEqual(FeedbackMapper.MapToDomain(result).Data, data);
        }

        [TestMethod]
        public void GetFeedback_should_return_notFound_with_wrong_id()
        {
            //var data = Fixture.Create<FeedbackData>();

            //MockRepository.DefaultValue = DefaultValue.Mock;
            //MockRepository.Setup(x =>
            //    x.Get(It.IsAny<int>())).ReturnsAsync(new Feedback());

            FeedbackMock.SetupUserFeedback(new Feedback());
            Controller = new FeedbackController(FeedbackMock);

            // Act
            var result2 = Controller.GetFeedback(Fixture.Create<int>()).GetAwaiter()
                .GetResult().Result;

            // Assert
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_okResult_if_request_correct_and_id_in_database()
        {
            var data = Fixture.Create<FeedbackData>();
            var entity = new Feedback(data);

            FeedbackMock.SetupUserFeedback(entity);
            var request = Fixture.Create<UpdateFeedbackRequest>();
            //MockRepository.Setup(x =>
            //    x.Update(entity)).Returns(Task.CompletedTask);
            //MockRepository.Setup(x =>
            //    x.Get(data.Id)).ReturnsAsync(entity);
            Controller = new FeedbackController(FeedbackMock);

            // Act
            var result1 = Controller.PutFeedback(data.Id,request).GetAwaiter().GetResult();
            
            // Assert
            Assert.IsTrue(FeedbackMock.VerifyUpdate());
            Assert.AreEqual(result1.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_badRequest_if_request_incorrect()
        {
            var entity = new Feedback();
            var request = new UpdateFeedbackRequest();

            MockRepository.Setup(x =>
                x.Update(entity)).Returns(Task.CompletedTask);
            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var result1 = Controller.PutFeedback(Fixture.Create<int>(), request)
                .GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_badRequest_if_entity_is_not_in_database()
        {
            var request = Fixture.Create<UpdateFeedbackRequest>();
            var id = Fixture.Create<int>();

            MockRepository.Setup(x =>
                x.Get(id)).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var result1 = Controller.PutFeedback(id, request)
                .GetAwaiter().GetResult();
            MockRepository.Verify(x => x.Get(id),Times.Once);

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_noContentResult_if_entity_is_in_database()
        {
            var id = Fixture.Create<int>();
            //MockRepository.Setup(x =>
            //    x.Delete(id)).Returns(Task.CompletedTask);
            //MockRepository.Setup(x =>
            //    x.Get(id)).ReturnsAsync(new Feedback(Fixture.Create<FeedbackData>()));
            var entity = new Feedback(Fixture.Create<FeedbackData>());
            entity.Data.Id = id;
            FeedbackMock.SetupUserFeedback(entity);
            
            Controller = new FeedbackController(FeedbackMock);

            // Act
            var result1 = Controller.DeleteFeedback(id).GetAwaiter().GetResult();

            // Assert
            Assert.IsTrue(FeedbackMock.VerifyDelete());
            Assert.AreEqual(result1.GetType(), typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_notFoundResult_if_entity_is_not_in_database()
        {
            var id = Fixture.Create<int>();
            MockRepository.Setup(x =>
                x.Get(It.IsAny<int>())).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var result2 = Controller.DeleteFeedback(Fixture.Create<int>()).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostFeedback_should_return_badRequest_if_request_values_null()
        {
            var request = new AddFeedbackRequest();
            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var response = Controller.PostFeedback(request).GetAwaiter().GetResult().Result;
         
            // Assert
            Assert.AreEqual(response.GetType(), typeof(BadRequestResult));
        }
        [TestMethod]
        public void PostFeedback_should_return_conflict_if_create_fails()
        {
            MockRepository.Setup(x =>
                x.Add(It.IsAny<Feedback>())).ReturnsAsync(new Feedback());

            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var result = Controller.PostFeedback(Fixture.Create<AddFeedbackRequest>()).GetAwaiter().GetResult().Result;
            
            // Assert
            Assert.AreEqual(result.GetType(),typeof(ConflictResult));
        }
        [TestMethod]
        public void PostFeedback_should_return_createdAtAction_if_request_successful()
        {
            MockRepository.Setup(x =>
                x.Add(It.IsAny<Feedback>())).ReturnsAsync(new Feedback(Fixture.Create<FeedbackData>()));

            Controller = new FeedbackController(MockRepository.Object);

            // Act
            var result = Controller.PostFeedback(Fixture.Create<AddFeedbackRequest>()).GetAwaiter().GetResult().Result;
            MockRepository.Verify(x => x.Add(It.IsAny<Feedback>()),Times.Once);

            // Assert
            Assert.AreEqual(result.GetType(),typeof(CreatedAtActionResult));
        }
    }
}
