using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain;
using PlaneStore.Infrastructure.Data;
using System.Linq.Expressions;

namespace PlaneStore.Infrastructure
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll() => dbSet.AsQueryable();

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
            => dbSet.Where(predicate);

        public virtual void Add(T entity) => dbSet.Add(entity);

        public virtual void AddRange(IEnumerable<T> entities) => dbSet.AddRange(entities);

        public virtual void Remove(T entity) => dbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities) => dbSet.RemoveRange(entities);

        public virtual void Update(T entity) => dbSet.Update(entity);

        public void Attach(object entity) => dbContext.Attach(entity);

        public void AttachRange(IEnumerable<object> entities) => dbContext.AttachRange(entities);

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
