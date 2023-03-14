using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public abstract class AdminControllerBase : Controller
    { }
}
