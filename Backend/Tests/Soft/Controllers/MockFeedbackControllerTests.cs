using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GetAllFeedbacks_should_return_all_feedbacks()
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
            mockRepository.Setup(x =>
                x.Get(It.IsNotIn(data.Id)).Result).Returns(new Feedback());
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result = ((FeedbackModel) ((OkObjectResult)
                Controller.GetFeedback(data.Id).GetAwaiter().GetResult().Result).Value);
            var result2 = Controller.GetFeedback(Fixture.Create<int>()).GetAwaiter()
                .GetResult().Result;

            // Assert
            AssertArePropertyValuesEqual(FeedbackMapper.MapToDomain(result).Data, data);
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteFeedback_should_return_noContentResult_if_successful_or_notFoundResult_if_unSuccessful()
        {
            //TODO Verify that delete method gets called

            var mockRepository = new Mock<IFeedbackRepository>();
            mockRepository.DefaultValue = DefaultValue.Mock;
            var id = Fixture.Create<int>();
            mockRepository.Setup(x =>
                x.Delete(id)).Returns(Task.CompletedTask);
            mockRepository.Setup(x =>
                x.Get(id)).ReturnsAsync(new Feedback(Fixture.Create<FeedbackData>()));
            mockRepository.Setup(x =>
                x.Get(It.IsNotIn(id))).ReturnsAsync(new Feedback());
            Controller = new FeedbackController(mockRepository.Object);

            // Act
            var result1 = Controller.DeleteFeedback(id).GetAwaiter().GetResult();
            var result2 = Controller.DeleteFeedback(Fixture.Create<int>()).GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(result1.GetType(), typeof(NoContentResult));
            Assert.AreEqual(result2.GetType(), typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostFeedback_should_return_badrequest_if_request_values_null()
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

            // Assert
            Assert.AreEqual(result.GetType(),typeof(CreatedAtActionResult));
        }
    }
}
