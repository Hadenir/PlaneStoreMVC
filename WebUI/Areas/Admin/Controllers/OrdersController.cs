using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Services;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    public class OrdersController : AdminControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public ViewResult Index()
        {
            var orders = _orderService.GetOrders().AsNoTracking();

            return View(new OrdersViewModel
            {
                AllOrders = orders,
                UndeliveredOrders = orders.Where(o => !o.IsDelivered),
                DeliveredOrders = orders.Where(o => o.IsDelivered),
            });
        }

        [HttpPost]
        public IActionResult Deliver(Guid? id)
        {
            var order = _orderService.GetOrderById(id);
            if (order is not null)
            {
                order.IsDelivered = true;
                _orderService.UpdateOrder(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Reset(Guid? id)
        {
            var order = _orderService.GetOrderById(id);
            if (order is not null)
            {
                order.IsDelivered = false;
                _orderService.UpdateOrder(order);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
