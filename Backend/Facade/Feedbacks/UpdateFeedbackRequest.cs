using System;

namespace Facade.Feedbacks
{
    public class UpdateFeedbackRequest
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
