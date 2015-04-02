using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TakeDocDataAccess
{
    public abstract class DaoGeneric<T> : IDaoGeneric<T> where T: class
    {
        protected System.Data.Entity.DbContext ctx;

        public ICollection<T> GetAll()
        {
            IQueryable<T> query = ctx.Set<T>();
            return query.ToList<T>();
        }

        public ICollection<T> GetBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = ctx.Set<T>().Where(where);

            foreach (Expression<Func<T, object>> property in properties)
            {
                query = query.Include<T,object>(property);
            }

            ICollection<T> retour = query.ToList();
            return retour;
        }

        public void Update(T item)
        {
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void Update(ICollection<T> items)
        {
            foreach (T item in items)
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            ctx.SaveChanges();
        }

        public void Delete(T item)
        {
            ctx.Entry(item).State = EntityState.Deleted;
            ctx.SaveChanges();
        }

        public void Delete(ICollection<T> items)
        {
            foreach (T item in items)
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            }
            ctx.SaveChanges();
        }
    }
}
