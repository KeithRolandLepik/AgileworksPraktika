using Facade.Common;
using System;

namespace Facade.Feedbacks
{
    public class FeedbackView : DefinedEntityView
    {
        public DateTime DateAdded { get; set; }
        public DateTime DueDate { get; set; }
        public Boolean Completed { get; set; }
        public Boolean Overdue { get; set; }
    }
}
