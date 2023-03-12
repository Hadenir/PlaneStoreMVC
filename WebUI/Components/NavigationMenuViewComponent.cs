using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using PlaneStore.Application.Services;
using PlaneStore.WebUI.Models.Components;

namespace PlaneStore.WebUI.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IManufacturerService _manufacturerService;

        public NavigationMenuViewComponent(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public ViewViewComponentResult Invoke(Guid? manufacturerId = null)
        {
            var model = new NavigationMenuViewModel
            {
                SelectedManufacturer = _manufacturerService.GetManufacturerById(manufacturerId),
                Manufacturers = _manufacturerService.GetManufacturers().OrderBy(m => m.Name),
            };

            return View(model);
        }
    }
}
