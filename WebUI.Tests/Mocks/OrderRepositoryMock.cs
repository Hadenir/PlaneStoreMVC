using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.WebUI.Tests.Mocks
{
    internal class OrderRepositoryMock : GenericRepositoryMock<Order>, IOrderRepository
    {
        public OrderRepositoryMock(IEnumerable<Order> entities) : base(entities, o => o.Id)
        { }
    }
}
