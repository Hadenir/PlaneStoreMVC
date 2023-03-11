using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.Infrastructure.Data;

namespace PlaneStore.Infrastructure.Repositories
{
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        { }

        public override Order? GetById(Guid? id)
            => dbSet.Include(o => o.Lines).ThenInclude(l => l.Aircraft).FirstOrDefault(o => o.Id == id);

        protected override void Attach(Order order)
        {
            // Let EF know that the aircraft inside order already exist.
            dbContext.AttachRange(order.Lines.Select(l => l.Aircraft));
        }
    }
}
