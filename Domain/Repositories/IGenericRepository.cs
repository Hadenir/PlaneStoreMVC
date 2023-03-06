using System.Linq.Expressions;

namespace PlaneStore.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(Guid? id);
        IQueryable<T> GetAll();
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

        void Commit();
    }
}
