using Microsoft.AspNetCore.Mvc;
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
                : _manufacturerRepository.GetById(manufacturerId);

            var model = new AircraftListViewModel
            {
                Aircraft = _aircraftRepository
                    .FindAll(a => manufacturerId == null || a.ManufacturerId == manufacturerId)
                    .OrderBy(a => a.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _aircraftRepository
                        .FindAll(a => manufacturerId == null || a.ManufacturerId == manufacturerId)
                        .Count(),
                },
                SelectedManufacturer = manufacturer,
            };

            return View(model);
        }
    }
}
