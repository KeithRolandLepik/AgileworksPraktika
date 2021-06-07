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
            var query = dbSet.AsQueryable();
            return toDomainObjectsList(await query.ToListAsync());
        }

        public async Task<TDomain> Get(int id)
        {
            if (id is default(int)) return new TDomain();

            var d = await getData(id);
            if (d is null) return unspecifiedEntity();

            var obj = toDomainObject(d);
            return obj;
        }


        public async Task Delete(int id)
        {

            if (id is default(int)) return;

            var d = await getData(id);

            if (d is null) return;
        
            try
            {
                dbSet.Remove(d);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException){} 
        }

        public async Task<TDomain> Add(TDomain obj)
        {
            if (obj?.Data is null) return new TDomain();

            try
            {
                dbSet.Add(obj.Data);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }
            
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

        protected async Task<TData> getData(int id)
        => await dbSet.FirstOrDefaultAsync(m => m.Id == id);

        protected TData getDataById(TData d)
            => dbSet.Find(d.Id);
   
        internal List<TDomain> toDomainObjectsList(List<TData> set) => set.Select(toDomainObject).ToList();

        protected TData getData(TDomain obj) => obj.Data;

        protected abstract TData copyData(TData d);

        protected internal abstract TDomain toDomainObject(TData UniqueEntityData);

        protected abstract TDomain unspecifiedEntity();

    }
}
