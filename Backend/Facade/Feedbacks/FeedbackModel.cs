using Facade.Common;
using System;

namespace Facade.Feedbacks
{
    public class FeedbackModel : DefinedEntityModel
    {
        public DateTime DateAdded { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
    }
}
