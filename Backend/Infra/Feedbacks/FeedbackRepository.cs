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
    public sealed class FeedbackRepository:  IFeedbackRepository
    {

        private IDocumentSession _documentSession;
        public FeedbackRepository([FromServices] IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        public async Task<Feedback> Add(Feedback obj)
        {
            _documentSession.Store(obj.Data);
            await _documentSession.SaveChangesAsync();
            var d = await _documentSession.Query<FeedbackData>().FirstOrDefaultAsync(x => x.Id == obj.Data.Id);
            return toDomainObject(d);
        }

        public async Task Delete(int id)
        {
            _documentSession.Delete(id);

            await _documentSession.SaveChangesAsync();
        }

        public async Task<List<Feedback>> Get()
        {
            var list = await _documentSession.Query<FeedbackData>().ToListAsync();

            return toDomainObjectsList(list);
        }

        public async Task<Feedback> Get(int id)
        {
            var d = await _documentSession.Query<FeedbackData>().FirstOrDefaultAsync(x => x.Id == id);

            return new Feedback(d);

        }

        public async Task Update(Feedback obj)
        {
            _documentSession.Store<FeedbackData>(obj.Data);

            await _documentSession.SaveChangesAsync();
        }


        private List<Feedback> toDomainObjectsList(IEnumerable<FeedbackData> datas)
        {
            var l = new List<Feedback>();
            foreach(var e in datas)
            {
                l.Add(toDomainObject(e));
            }
            return l;
        }

        private Feedback toDomainObject(FeedbackData d) => new Feedback(d);
    }
}
