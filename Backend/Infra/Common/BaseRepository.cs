using Data.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infra.Common
{
    public abstract class BaseRepository<TDomain, TData> :
            ICrudMethods<TDomain> where TData : UniqueEntityData, new()
            where TDomain : Entity<TData>, new()
    {
        protected internal DbContext db;
        protected internal DbSet<TData> dbSet;

        protected BaseRepository(DbContext c, DbSet<TData> s)
        {
            db = c;
            dbSet = s;
        }

        public virtual async Task<List<TDomain>> Get()
        {
            var query = createSqlQuery();
            var set = await runSqlQueryASync(query);

            return toDomainObjectsList(set);
        }

        internal async Task<List<TData>> runSqlQueryASync(IQueryable<TData> query) => await query.AsNoTracking().ToListAsync();

        protected internal virtual IQueryable<TData> createSqlQuery()
        {
            var query = from s in dbSet select s;
            return query;
        }

        public async Task<TDomain> Get(int id)
        {
            if (id is default(int)) return new TDomain();

            var d = await getData(id);
            if (d is null) return unspecifiedEntity();

            var obj = toDomainObject(d);
            return obj;
        }

        protected TData getData(TDomain obj) => obj.Data;


        public async Task Delete(int id)
        {
            if (id is default(int)) return;

            var d = await getData(id);

            if (d is null) return;
            dbSet.Remove(d);
            await db.SaveChangesAsync();
        }

        public async Task<TDomain> Add(TDomain obj)
        {
            if (obj?.Data is null) return new TDomain();
            dbSet.Add(obj.Data);
            await db.SaveChangesAsync();
            
            var o = await Get(obj.Data.Id);
            
            return o;
        }

        public async Task Update(TDomain obj)
        {
            if(obj.Data is null)
            {
                return;
            }
            var d = getData(obj);
            d = copyData(d);

            
            db.Attach(d).State = EntityState.Modified;

            try { await db.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {

            }

        }

        protected abstract TDomain unspecifiedEntity();

        public object GetById(int id) => Get(id).GetAwaiter().GetResult(); 

        protected async Task<TData> getData(int id)
        => await dbSet.FirstOrDefaultAsync(m => m.Id == id);
        protected abstract TData copyData(TData d);

        protected TData getDataById(TData d)
            => dbSet.Find(d.Id);
        internal List<TDomain> toDomainObjectsList(List<TData> set) => set.Select(toDomainObject).ToList();

        protected internal abstract TDomain toDomainObject(TData UniqueEntityData);

    }
}
