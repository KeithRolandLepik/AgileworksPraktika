using Data.Feedbacks;
using Domain.Feedbacks;
using Infra.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Feedbacks
{
    public sealed class FeedbackRepository : BaseRepository<Feedback, FeedbackData>, IFeedbackRepository
    {
        public FeedbackRepository(FeedbackDbContext c) : base(c, c.FeedbackDatas) { }

        protected internal override Feedback toDomainObject(FeedbackData d) => new Feedback(d);
        protected override Feedback unspecifiedEntity() => new Feedback();

        protected override FeedbackData copyData(FeedbackData d)
        {
            var x = getDataById(d);

            if (x is null) return d;

            x.Id = d.Id;
            x.DueDate = d.DueDate;
            x.DateAdded = d.DateAdded;
            x.Overdue = d.Overdue;
            x.Completed = d.Completed;
            x.Description = d.Description;

            return x;
        }

    }
}
