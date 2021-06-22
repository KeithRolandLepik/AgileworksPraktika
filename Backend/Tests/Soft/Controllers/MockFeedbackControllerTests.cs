using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Baseline.Reflection;
using Data.Feedbacks;
using Domain.Feedbacks;
using Facade.Feedbacks;
using Infra.Feedbacks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NuGet.Frameworks;
using Soft.Controllers;
using FeedbackData = Data.Feedbacks.FeedbackData;

namespace Tests.Soft.Controllers
{
    [TestClass]
    public class MockFeedbackControllerTests : BaseTests
    {
        internal FeedbackController Controller;

        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
        }

        [TestMethod]
        public void GetFeedbacks_should_return_all_feedbacks()
        {
            var mockRepository = new Mock<IFeedbackRepository>();
            var l = new List<Feedback>();
            for (int i = 0; i < 5; i++)
            {
                l.Add(new Feedback(Fixture.Create<FeedbackData>()));
            }

            mockRepository.Setup(x => x.Get().Result).Returns(l);
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result = Controller.GetFeedbacks().GetAwaiter().GetResult().Value;

            // Assert
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void GetFeedback_should_return_feedback_with_given_id()
        {
            var mockRepository = new Mock<IFeedbackRepository>();
            var data = Fixture.Create<FeedbackData>();
            var entity = new Feedback(data);

            mockRepository.DefaultValue = DefaultValue.Mock;
            mockRepository.Setup(x =>
                x.Get(data.Id)).Returns(Task.FromResult(entity));
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result = ((FeedbackModel) ((OkObjectResult)
                Controller.GetFeedback(data.Id).GetAwaiter().GetResult().Result).Value);

            // Assert
            AssertArePropertyValuesEqual(FeedbackMapper.MapToDomain(result).Data, data);
        }

        [TestMethod]
        public void GetFeedback_should_return_notFound_with_wrong_id()
        {
            var mockRepository = new Mock<IFeedbackRepository>();
            var data = Fixture.Create<FeedbackData>();

            mockRepository.DefaultValue = DefaultValue.Mock;
            mockRepository.Setup(x =>
                x.Get(It.IsAny<int>())).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result2 = Controller.GetFeedback(Fixture.Create<int>()).GetAwaiter()
                .GetResult().Result;

            // Assert
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_okResult_if_request_correct_and_id_in_database()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            var data = Fixture.Create<FeedbackData>();
            var entity = new Feedback(data);
            var request = Fixture.Create<UpdateFeedbackRequest>();
            mockRepository.Setup(x =>
                x.Update(entity)).Returns(Task.CompletedTask);
            mockRepository.Setup(x =>
                x.Get(data.Id)).ReturnsAsync(entity);
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result1 = Controller.PutFeedback(data.Id,request).GetAwaiter().GetResult();
            mockRepository.Verify(x => x.Update(entity), Times.Once);

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(OkResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_badRequest_if_request_incorrect()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            var entity = new Feedback();
            var request = new UpdateFeedbackRequest();

            mockRepository.Setup(x =>
                x.Update(entity)).Returns(Task.CompletedTask);
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result1 = Controller.PutFeedback(Fixture.Create<int>(), request)
                .GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutFeedback_should_return_badRequest_if_entity_is_not_in_database()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            var request = Fixture.Create<UpdateFeedbackRequest>();
            var id = Fixture.Create<int>();

            mockRepository.Setup(x =>
                x.Get(id)).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result1 = Controller.PutFeedback(id, request)
                .GetAwaiter().GetResult();
            mockRepository.Verify(x => x.Get(id),Times.Once);

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_noContentResult_if_entity_is_in_database()
        {

            var mockRepository = new Mock<IFeedbackRepository> {DefaultValue = DefaultValue.Mock};
            var id = Fixture.Create<int>();
            mockRepository.Setup(x =>
                x.Delete(id)).Returns(Task.CompletedTask);
            mockRepository.Setup(x =>
                x.Get(id)).ReturnsAsync(new Feedback(Fixture.Create<FeedbackData>()));
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result1 = Controller.DeleteFeedback(id).GetAwaiter().GetResult();
            mockRepository.Verify(x => x.Delete(id), Times.Once);

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_notFoundResult_if_entity_is_not_in_database()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            var id = Fixture.Create<int>();
            mockRepository.Setup(x =>
                x.Get(It.IsAny<int>())).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result2 = Controller.DeleteFeedback(Fixture.Create<int>()).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostFeedback_should_return_badRequest_if_request_values_null()
        {
            var mockRepository = new Mock<IFeedbackRepository> {DefaultValue = DefaultValue.Mock};
            var request = new AddFeedbackRequest();
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var response = Controller.PostFeedback(request).GetAwaiter().GetResult().Result;
         
            // Assert
            Assert.AreEqual(response.GetType(), typeof(BadRequestResult));
        }
        [TestMethod]
        public void PostFeedback_should_return_conflict_if_create_fails()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            mockRepository.Setup(x =>
                x.Add(It.IsAny<Feedback>())).ReturnsAsync(new Feedback());

            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result = Controller.PostFeedback(Fixture.Create<AddFeedbackRequest>()).GetAwaiter().GetResult().Result;
            
            // Assert
            Assert.AreEqual(result.GetType(),typeof(ConflictResult));
        }
        [TestMethod]
        public void PostFeedback_should_return_createdAtAction_if_request_successful()
        {
            var mockRepository = new Mock<IFeedbackRepository> { DefaultValue = DefaultValue.Mock };
            mockRepository.Setup(x =>
                x.Add(It.IsAny<Feedback>())).ReturnsAsync(new Feedback(Fixture.Create<FeedbackData>()));

            Controller = new FeedbackController(mockRepository.Object);
            // Act
            var result = Controller.PostFeedback(Fixture.Create<AddFeedbackRequest>()).GetAwaiter().GetResult().Result;
            mockRepository.Verify(x => x.Add(It.IsAny<Feedback>()),Times.Once);

            // Assert
            Assert.AreEqual(result.GetType(),typeof(CreatedAtActionResult));
        }
    }
}
