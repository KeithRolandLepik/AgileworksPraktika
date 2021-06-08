﻿using Data.Feedbacks;
using System;

namespace Tests
{
    public static class GetRandom
    {
        public static Boolean Bool()
        {
            Random rng = new Random();
            var rndNr = rng.Next(0, 1);
            if (rndNr == 0)
            {
                return true;
            }
            return false;
        }
        public static DateTime Datetime()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random rng = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(rng.Next(range));
        }
        public static int RndInteger(int min, int max)
        {
            Random rng = new Random();
            return rng.Next(min, max);
        }
        public static FeedbackData FeedbackData()
        {
            return new FeedbackData { Description = "test", Completed = Bool(), DateAdded = DateTime.Now, DueDate = Datetime(), Overdue = Bool() };
        }
    }
}
