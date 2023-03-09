using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;
using System.Linq.Expressions;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        public virtual T? GetById(Guid? id) => dbSet.Find(id);

        public virtual IQueryable<T> GetAll() => dbSet.AsQueryable();

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate) => dbSet.Where(predicate);

        public virtual void Add(T entity) => dbSet.Add(entity);

        public virtual void AddRange(IEnumerable<T> entities) => dbSet.AddRange(entities);

        public virtual void Remove(T entity) => dbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities) => dbSet.RemoveRange(entities);

        public virtual void Update(T entity) => dbSet.Update(entity);

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
