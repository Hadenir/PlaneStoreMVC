using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;

namespace PlaneStore.Application.Services
{
    public interface IOrderService
    {
        public Guid PlaceOrder(Order order);
    }

    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Guid PlaceOrder(Order order)
        {
            if (!order.Lines.Any())
            {
                throw new Exception("Cannot place an empty order");
            }

            _orderRepository.Update(order);
            _orderRepository.Commit();

            return order.Id;
        }
    }
}
