using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using Marten;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Feedbacks
{
    public sealed class FeedbackRepository:  BaseRepository<Feedback, FeedbackData>, IFeedbackRepository
    {
        public FeedbackRepository(IDocumentSession session): base(session) { }

        protected override FeedbackData copyData(FeedbackData data)
        {
            var x = new FeedbackData();
            x.Id = data.Id;
            x.DueDate = data.DueDate;
            x.Description = data.Description;
            x.DateAdded = data.DateAdded;
            x.Completed = data.Completed;
            x.Overdue = data.Overdue;
            return x;
        }

        protected internal override Feedback toDomainObject(FeedbackData d) => new Feedback(d);


    }
}
