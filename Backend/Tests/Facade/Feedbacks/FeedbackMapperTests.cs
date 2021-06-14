using Domain.Feedbacks;
using Facade.Feedbacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Facade.Feedbacks
{
    [TestClass]
    public class FeedbackMapperTests : BaseTests
    {

        [TestMethod]
        public void MapToDomain_should_map_view_to_domain()
        {
            var view = new FeedbackModel { IsOverdue = GetRandom.Bool(), IsCompleted = GetRandom.Bool(),
              DateAdded = GetRandom.Datetime(), Description = "test", DueDate = GetRandom.Datetime() };
          
            // Act
            var data = FeedbackMapper.MapToDomain(view).Data;

            // Assert
            AssertArePropertyValuesEqual(view, data);
        }

        [TestMethod]
        public void MapToView_should_map_domain_to_view()
        {
            var data = GetRandom.FeedbackData();

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
                Description = "test",
                DueDate = GetRandom.Datetime()
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
                Description = "test",
                DueDate = GetRandom.Datetime()
            };
            var domain = new Feedback();
            domain.Data = GetRandom.FeedbackData();

            // Act
            domain = FeedbackMapper.MapToDomainFromUpdateRequest(domain, updateData);

            // Assert
            AssertArePropertyValuesEqual(updateData, domain.Data);
        }
    }
}
