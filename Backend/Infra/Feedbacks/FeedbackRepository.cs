using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Marten;

namespace Infra.Feedbacks
{
    public sealed class FeedbackRepository:  BaseRepository<Feedback, FeedbackData>, IFeedbackRepository
    {
        public FeedbackRepository(IDocumentSession session): base(session) { }

        protected override FeedbackData copyData(FeedbackData data)
        {
            return 
                new FeedbackData {
                    Id = data.Id,
                    DueDate = data.DueDate,
                    Description = data.Description,
                    DateAdded = data.DateAdded,
                    Completed = data.Completed,
                    Overdue = data.Overdue
                };
        }

        protected internal override Feedback toDomainObject(FeedbackData entityData) => new Feedback(entityData);
    }
}
