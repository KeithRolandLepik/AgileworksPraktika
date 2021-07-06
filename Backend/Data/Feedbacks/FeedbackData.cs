using Data.Common;
using System;

namespace Data.Feedbacks
{
    public class FeedbackData : DefinedEntityData
    {
        
        //public void Complete()
        //{
        //    IsCompleted = true;
        //}
        //public FeedbackData Complete2()
        //{
        //    var clone = (FeedbackData)MemberwiseClone();
        //    clone.IsCompleted = true;
        //    return clone;
        //}

        public DateTime DateAdded { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue => DueDate < DateTime.Now.AddHours(1);
    }
}
