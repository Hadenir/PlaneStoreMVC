using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly Cart _cart;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(Cart cart, IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _cart = cart;
            _mapper = mapper;
        }

        public ViewResult Checkout() => View(new OrderViewModel());

        [HttpPost]
        public IActionResult Checkout(OrderViewModel orderModel)
        {
            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(orderModel);
                order.Lines = _mapper.Map<List<OrderLine>>(_cart.Lines);
                var orderId = _orderService.PlaceOrder(order);

                _cart.Clear();
                return RedirectToAction("Completed", new { orderId });
            }

            return View();
        }

        public ViewResult Completed(Guid orderId) => View(orderId);
    }
}
