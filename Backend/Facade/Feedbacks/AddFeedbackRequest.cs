using System;

namespace Facade.Feedbacks
{
    public class AddFeedbackRequest
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
