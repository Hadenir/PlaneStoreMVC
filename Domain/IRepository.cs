using System.Linq.Expressions;

namespace PlaneStore.Domain
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

        void Attach(object entity);
        void AttachRange(IEnumerable<object> entities);

        void Commit();
    }
}
