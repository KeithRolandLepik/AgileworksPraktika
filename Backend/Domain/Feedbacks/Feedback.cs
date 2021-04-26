using Data.Feedbacks;
using Domain.Common;
using System;

namespace Domain.Feedbacks
{
    public sealed class Feedback : Entity<FeedbackData>
    {
        public Feedback() : this(null) { }
        public Feedback(FeedbackData data) : base(data) 
        {
            if(data != null) 
            {
                CalculateAndSetOverDue();
            }
        }
        private double GetHoursTillDueDate()
        {
            var today = DateTime.Now;
            var dueDate = Data.DueDate;
            return (dueDate - today).TotalHours;
        }
        private void CalculateAndSetOverDue()
        {
            if (GetHoursTillDueDate() > 1)
            {
                Data.Overdue = false;
            }
            Data.Overdue = true;
        }
    }
}
