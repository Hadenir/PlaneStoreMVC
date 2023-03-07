using PlaneStore.Domain.Repositories;
using System.Linq.Expressions;

namespace PlaneStore.WebUI.Tests.Mocks
{
    internal class GenericRepositoryMock<T> : IGenericRepository<T> where T : class
    {
        public List<T> Entities { get; private init; }

        private readonly Func<T, Guid> _primaryKey;

        public GenericRepositoryMock(IEnumerable<T> entities, Func<T, Guid> primaryKey)
        {
            Entities = entities.ToList();
            _primaryKey = primaryKey;
        }

        public T? GetById(Guid? id) => Entities.FirstOrDefault(a => _primaryKey.Invoke(a) == id);

        public IQueryable<T> GetAll() => Entities.AsQueryable();

        public IQueryable<T> FindAll(Expression<Func<T, bool>> expression) => Entities.Where(expression.Compile()).AsQueryable();

        public void Add(T entity) => Entities.Add(entity);

        public void AddRange(IEnumerable<T> entities) => Entities.AddRange(entities);

        public void Remove(T entity) => Entities.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => Entities.RemoveAll(entities.Contains);

        public void Update(T entity)
        {
            var idx = Entities.IndexOf(entity);
            if (idx >= 0)
                Entities[idx] = entity;
        }

        public void Commit() { }
    }
}
