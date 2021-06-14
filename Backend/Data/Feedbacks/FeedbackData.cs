using Data.Common;
using System;

namespace Data.Feedbacks
{
    public class FeedbackData : DefinedEntityData
    {
        public DateTime DateAdded { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
    }
}
