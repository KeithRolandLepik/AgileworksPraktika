using Data.Common;
using Domain.Common;
using Marten;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Common
{
    public abstract class BaseRepository<TDomain, TData> :
            ICrudMethods<TDomain> where TData : UniqueEntityData, new()
            where TDomain : Entity<TData>, new()
    {
        internal readonly IDocumentStore _store;
        
        protected BaseRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<List<TDomain>> Get()
        {
            await using var session = _store.LightweightSession();

            var list = await session.Query<TData>().ToListAsync();
            return ToDomainObjectsList(list);
        }

        public async Task<TDomain> Get(int id)
        {
            await using var session = _store.LightweightSession();

            var entityData = session.Load<TData>(id);
            
            return ToDomainObject(entityData);
        }

        public async Task Delete(int id)
        {
            await using var session = _store.LightweightSession();

            session.Delete<TData>(id);
            await session.SaveChangesAsync();
        }

        public async Task<TDomain> Add(TDomain obj)
        {
            await using var session = _store.LightweightSession();

            session.Store(obj.Data);
            
            await session.SaveChangesAsync();

            var entityData = await session.Query<TData>().FirstOrDefaultAsync(x => x.Id == obj.Data.Id);
            
            return ToDomainObject(entityData);
        }

        public async Task Update(TDomain obj)
        {
            await using var session = _store.LightweightSession();

            session.Store(obj.Data);

            await session.SaveChangesAsync();
        }

        
        internal List<TDomain> ToDomainObjectsList(IEnumerable<TData> set) => set.Select(ToDomainObject).ToList();

        protected internal abstract TDomain ToDomainObject(TData entityData);

    }
}
