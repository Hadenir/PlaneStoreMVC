using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly Cart _cart;

        public CartController(IAircraftRepository aircraftRepository, Cart cart)
        {
            _aircraftRepository = aircraftRepository;
            _cart = cart;
        }

        public IActionResult Index(string returnUrl = "/")
        {
            return View(new CartViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public IActionResult Add(Guid aircraftId, string returnUrl)
        {
            var aircraft = _aircraftRepository.GetById(aircraftId);
            if (aircraft is not null)
            {
                _cart.AddItem(aircraft, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
