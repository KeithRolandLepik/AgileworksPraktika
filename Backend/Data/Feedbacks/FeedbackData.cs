using Data.Common;
using System;

namespace Data.Feedbacks
{
    public class FeedbackData : DefinedEntityData
    {
        public DateTime DateAdded { get; set; }
        public DateTime DueDate { get; set; }
        public Boolean Completed { get; set; }
        public Boolean Overdue { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
