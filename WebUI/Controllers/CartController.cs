using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Application.Services;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IAircraftService _aircraftService;
        private readonly Cart _cart;

        public CartController(IAircraftService aircraftService, Cart cart)
        {
            _aircraftService = aircraftService;
            _cart = cart;
        }

        public ViewResult Index(string returnUrl = "/")
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
            var aircraft = _aircraftService.GetAircraftById(aircraftId);
            if (aircraft is not null)
            {
                _cart.AddItem(aircraft, 1);
            }

            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        [HttpPost]
        public IActionResult Remove(Guid aircraftId, string returnUrl)
        {
            var aircraft = _aircraftService.GetAircraftById(aircraftId);
            if (aircraft is not null)
            {
                _cart.RemoveItem(aircraft);
            }

            return RedirectToAction(nameof(Index), new { returnUrl });
        }
    }
}
