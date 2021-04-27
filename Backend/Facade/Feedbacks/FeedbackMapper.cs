﻿using Data.Feedbacks;
using Domain.Feedbacks;
using System;

namespace Facade.Feedbacks
{
    public static class FeedbackMapper
    {
        public static Feedback MapToDomain(FeedbackView v)
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

        public static FeedbackView MapToView(Feedback o)
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
        public static Feedback MapToDomainFromInput(FeedbackInput input)
        {
            var d = new FeedbackData
            {
                Description = input.Description,
                DueDate = input.DueDate,
                DateAdded = DateTime.Now
            };
            return new Feedback(d);
        }
        public static Feedback MapToDomainFromUpdate(FeedbackUpdate update)
        {
            var d = new FeedbackData
            {
                Description = update.Description,
                DueDate = update.DueDate,
                Completed = update.Completed,
                DateAdded = DateTime.Now,
            };
            return new Feedback(d);
        }
    }
}