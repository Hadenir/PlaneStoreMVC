using Microsoft.AspNetCore.Mvc;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AircraftController : Controller
    {
        public ViewResult Index() => View();
    }
}
