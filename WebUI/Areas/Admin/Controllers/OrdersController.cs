using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public ViewResult Index()
        {
            var allOrders = _orderRepository
                .GetAll()
                .Include(o => o.Lines)
                .ThenInclude(l => l.Aircraft)
                .ToList();

            return View(new OrdersViewModel
            {
                AllOrders = allOrders,
                UndeliveredOrders = allOrders.Where(o => !o.IsDelivered),
                DeliveredOrders = allOrders.Where(o => o.IsDelivered),
            });
        }

        [HttpPost]
        public IActionResult Deliver(Guid orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order is not null)
            {
                order.IsDelivered = true;
                _orderRepository.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reset(Guid orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order is not null)
            {
                order.IsDelivered = false;
                _orderRepository.Commit();
            }

            return RedirectToAction("Index");
        }
    }
}
