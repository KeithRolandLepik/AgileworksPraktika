using Data.Feedbacks;
using Domain.Feedbacks;

namespace Facade.Feedbacks
{
    public static class FeedbackViewFactory
    {
        public static Feedback Create(FeedbackView v)
        {
            var d = new FeedbackData
            {
                Id = v.Id,
                Description = v.Description,
                DueDate = v.DueDate,
                Completed = v.Completed,
                Overdue = v.Overdue,
                DateAdded = v.DateAdded,
            };
            
            return new Feedback(d);
        }

        public static FeedbackView Create(Feedback o)
        {
            var v = new FeedbackView
            {
                Id = o.Data.Id,
                Description = o.Data.Description,
                DueDate = o.Data.DueDate,
                Completed = o.Data.Completed,
                Overdue = o.Data.Overdue,
                DateAdded = o.Data.DateAdded,
            };
            return v;
        }
    }
}
