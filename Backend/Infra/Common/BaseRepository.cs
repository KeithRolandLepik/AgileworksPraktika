using Data.Common;
using Domain.Common;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Common
{
    public abstract class BaseRepository<TDomain, TData> :
            ICrudMethods<TDomain> where TData : UniqueEntityData, new()
            where TDomain : Entity<TData>, new()
    {
        private IDocumentSession _documentSession;
        

        protected BaseRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<List<TDomain>> Get()
        {
            var list = await _documentSession.Query<TData>().ToListAsync();

            return toDomainObjectsList(list);
        }

        public async Task<TDomain> Get(int id)
        {
            var d = await _documentSession.Query<TData>().FirstOrDefaultAsync(x => x.Id == id);

            return toDomainObject(d);
        }


        public async Task Delete(int id)
        {
            _documentSession.Delete<TData>(id);
            await _documentSession.SaveChangesAsync();
        }

        public async Task<TDomain> Add(TDomain obj)
        {
            _documentSession.Store(obj.Data);
            
            await _documentSession.SaveChangesAsync();

            var d = await _documentSession.Query<TData>().FirstOrDefaultAsync(x => x.Id == obj.Data.Id);
            
            return toDomainObject(d);
        }

        public async Task Update(TDomain obj)
        {
            _documentSession.Store<TData>(obj.Data);

            await _documentSession.SaveChangesAsync();
        }

        protected abstract TData copyData(TData data);

        internal List<TDomain> toDomainObjectsList(IEnumerable<TData> set) => set.Select(toDomainObject).ToList();

        protected internal abstract TDomain toDomainObject(TData UniqueEntityData);

    }
}
