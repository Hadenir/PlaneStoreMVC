using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Services;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize = 4;

        private readonly IAircraftService _aircraftService;
        private readonly IManufacturerService _manufacturerService;

        public HomeController(IAircraftService aircraftService, IManufacturerService manufacturerService)
        {
            _aircraftService = aircraftService;
            _manufacturerService = manufacturerService;
        }

        public ViewResult Index(Guid? manufacturerId = null, int currentPage = 1)
        {
            var manufacturer = manufacturerId is null
                ? null
                : _manufacturerService.GetManufacturerById(manufacturerId);

            var aircraftFiltered = _aircraftService.GetAircraft()
                    .Where(a => manufacturerId == null || a.ManufacturerId == manufacturerId);

            var model = new HomeViewModel
            {
                Aircraft = aircraftFiltered
                    .OrderBy(a => a.Id)
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = PageSize,
                    TotalItems = aircraftFiltered.Count(),
                },
                SelectedManufacturer = manufacturer,
            };

            return View(model);
        }
    }
}
