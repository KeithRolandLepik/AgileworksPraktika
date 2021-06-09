using System;

namespace Facade.Feedbacks
{
    public class FeedbackUpdate
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Boolean Completed { get; set; }
    }
}
