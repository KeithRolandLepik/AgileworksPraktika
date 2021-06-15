using Data.Feedbacks;
using Domain.Common;

namespace Domain.Feedbacks
{
    public sealed class Feedback : Entity<FeedbackData>
    {
        public Feedback() : this(null) { }
        public Feedback(FeedbackData data) : base(data) { }
    }
}
