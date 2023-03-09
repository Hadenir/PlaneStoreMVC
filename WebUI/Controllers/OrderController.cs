using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly Cart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(Cart cart, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
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
                _orderRepository.Update(order);
                _orderRepository.Commit();

                _cart.Clear();
                return RedirectToAction("Completed", new { orderId = order.Id });
            }

            return View();
        }

        public ViewResult Completed(Guid orderId) => View(orderId);
    }
}
