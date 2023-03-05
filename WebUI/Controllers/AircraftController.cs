using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class AircraftController : Controller
    {
        public int PageSize = 4;

        private readonly IAircraftRepository _aircraftRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public AircraftController(IAircraftRepository repository, IManufacturerRepository manufacturerRepository)
        {
            _aircraftRepository = repository;
            _manufacturerRepository = manufacturerRepository;
        }

        public IActionResult List(Guid? manufacturerId = null, int page = 1)
        {
            Manufacturer? manufacturer = manufacturerId is null
                ? null
                : _manufacturerRepository.Manufacturers.FirstOrDefault(m => m.Id == manufacturerId);

            var model = new AircraftListViewModel
            {
                Aircraft = _aircraftRepository.Aircraft
                    .Where(a => manufacturerId == null || a.ManufacturerId == manufacturerId)
                    .OrderBy(a => a.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _aircraftRepository.Aircraft.Count(),
                },
                SelectedManufacturer = manufacturer,
            };

            return View(model);
        }
    }
}
