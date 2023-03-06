using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models.Components;

namespace PlaneStore.WebUI.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public NavigationMenuViewComponent(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public IViewComponentResult Invoke(Guid? manufacturerId = null)
        {
            var selectedManufacturer = _manufacturerRepository.GetById(manufacturerId);
            var model = new NavigationMenuViewModel
            {
                SelectedManufacturer = selectedManufacturer,
                Manufacturers = _manufacturerRepository.GetAll().OrderBy(m => m.Name),
            };

            return View(model);
        }
    }
}
