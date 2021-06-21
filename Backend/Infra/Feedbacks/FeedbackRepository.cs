using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Marten;

namespace Infra.Feedbacks
{
    public sealed class FeedbackRepository:  BaseRepository<Feedback, FeedbackData>, IFeedbackRepository
    {
        public FeedbackRepository(IDocumentStore session): base(session) { }

        protected internal override Feedback ToDomainObject(FeedbackData entityData) => new(entityData);
    }
}
