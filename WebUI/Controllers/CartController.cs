using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Utilities;

namespace PlaneStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IAircraftRepository _aircraftRepository;

        public CartController(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public IActionResult Index(string returnUrl = "/")
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new();

            return View(new CartViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public IActionResult Add(Guid aircraftId, string returnUrl)
        {
            var aircraft = _aircraftRepository.GetById(aircraftId);
            if (aircraft is not null)
            {
                var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new();
                cart.AddItem(aircraft, 1);
                HttpContext.Session.SetJson("cart", cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}
