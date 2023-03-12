using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Utilities;
using PlaneStore.Domain;
using PlaneStore.Domain.Entities;

namespace PlaneStore.Application.Services
{
    public interface IOrderService
    {
        IQueryable<Order> GetOrders();
        Order? GetOrderById(Guid? id);
        public Guid PlaceOrder(Order order);
        public void UpdateOrder(Order order);
        public void RemoveOrderById(Guid id);
    }

    internal class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IQueryable<Order> GetOrders()
            => _orderRepository.GetAll()
                .Include(o => o.Lines)
                    .ThenInclude(l => l.Aircraft);

        public Order? GetOrderById(Guid? id)
            => GetOrders().FirstOrDefault(o => o.Id == id);

        public Guid PlaceOrder(Order order)
        {
            if (new[] { order.FullName, order.Street, order.City, order.PostCode, order.Country }.Any(string.IsNullOrEmpty))
            {
                throw new ServiceException("Cannot place empty with missing details");
            }
            if (!order.Lines.Any())
            {
                throw new ServiceException("Cannot place an empty order");
            }
            if (order.Lines.Any(l => l.Quantity <= 0))
            {
                throw new ServiceException("Order items' quantities must be positive");
            }

            // Let EF know that the aircraft inside order already exist.
            _orderRepository.AttachRange(order.Lines.Select(l => l.Aircraft));
            _orderRepository.Update(order);
            _orderRepository.Commit();

            return order.Id;
        }

        public void UpdateOrder(Order order)
        {
            if (order.Id == Guid.Empty)
            {
                throw new ServiceException("Cannot update order without specified id");
            }

            _orderRepository.Update(order);
            _orderRepository.Commit();
        }

        public void RemoveOrderById(Guid id)
        {
            var order = GetOrderById(id) ?? throw new ServiceException("Cannot remove nonexisting order");

            _orderRepository.Remove(order);
            _orderRepository.Commit();
        }
    }
}
