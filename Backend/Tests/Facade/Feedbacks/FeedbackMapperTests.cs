using System;
using AutoFixture;
using Data.Feedbacks;
using Domain.Feedbacks;
using Facade.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Feedbacks
{
    [TestClass]
    public class FeedbackMapperTests : BaseTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Fixture = new Fixture();
        }
        [TestMethod]
        public void MapToDomain_should_map_view_to_domain()
        {
            var view = new FeedbackModel { IsOverdue = false, IsCompleted = Fixture.Create<bool>(),
              DateAdded = Fixture.Create<DateTime>(), Description = Fixture.Create<string>(), DueDate = Fixture.Create<DateTime>()
            };
          
            // Act
            var data = FeedbackMapper.MapToDomain(view).Data;
            view.IsOverdue = data.IsOverdue;
        
            // Assert
            AssertArePropertyValuesEqual(view, data);
        }

        [TestMethod]
        public void MapToView_should_map_domain_to_view()
        {
            var data = Fixture.Create<FeedbackData>();

            // Act
            var view = FeedbackMapper.MapToModel(new Feedback(data));

            // Assert
            AssertArePropertyValuesEqual(view, data);
        }

        [TestMethod]
        public void MapToDomainFromInput_should_map_domain_from_input()
        {
            var inputData = new AddFeedbackRequest()
            {
                Description = Fixture.Create<string>(),
                DueDate = Fixture.Create<DateTime>(),
            };

            // Act
            var domain = FeedbackMapper.MapToDomainFromAddRequest(inputData);

            // Assert
            AssertArePropertyValuesEqual(inputData, domain.Data);
        }

        [TestMethod]
        public void MapToDomainFromUpdate_should_map_domain_from_input()
        {
            var updateData = new UpdateFeedbackRequest()
            {
                IsCompleted = false,
                Description = Fixture.Create<string>(),
                DueDate = Fixture.Create<DateTime>()
            };
            var domain = new Feedback();
            domain.Data = Fixture.Create<FeedbackData>();

            // Act
            domain = FeedbackMapper.MapToDomainFromUpdateRequest(domain, updateData);

            // Assert
            AssertArePropertyValuesEqual(updateData, domain.Data);
        }
    }
}
