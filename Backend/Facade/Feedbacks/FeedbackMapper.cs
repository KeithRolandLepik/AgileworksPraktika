using Data.Feedbacks;
using Domain.Feedbacks;
using System;

namespace Facade.Feedbacks
{
    public static class FeedbackMapper
    {
        public static Feedback MapToDomain(FeedbackModel feedbackModel) =>
            new(new FeedbackData {
                Id = feedbackModel.Id,
                Description = feedbackModel.Description,
                DueDate = feedbackModel.DueDate,
                IsCompleted = feedbackModel.IsCompleted,
                DateAdded = feedbackModel.DateAdded,
            });

        public static FeedbackModel MapToModel(Feedback feedback)
        {
            var v = new FeedbackModel
            {
                Id = feedback.Data.Id,
                Description = feedback.Data.Description,
                DueDate = feedback.Data.DueDate,
                IsCompleted = feedback.Data.IsCompleted,
                IsOverdue = feedback.Data.IsOverdue,
                DateAdded = feedback.Data.DateAdded,
            };
            return v;
        }
        public static Feedback MapToDomainFromAddRequest(AddFeedbackRequest addFeedbackRequest)
        {
            return new(new FeedbackData
            {
                Description = addFeedbackRequest.Description,
                DueDate = addFeedbackRequest.DueDate,
                DateAdded = DateTime.Now
            });
        }
        public static Feedback MapToDomainFromUpdateRequest(Feedback feedbackToUpdate, UpdateFeedbackRequest request)
        {
            feedbackToUpdate.Data.Description = request.Description;
            feedbackToUpdate.Data.DueDate = request.DueDate;
            feedbackToUpdate.Data.IsCompleted = request.IsCompleted;

            return feedbackToUpdate;
        }
    }
}
