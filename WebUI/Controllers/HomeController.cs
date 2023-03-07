using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize = 4;

        private readonly IAircraftRepository _aircraftRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public HomeController(IAircraftRepository repository, IManufacturerRepository manufacturerRepository)
        {
            _aircraftRepository = repository;
            _manufacturerRepository = manufacturerRepository;
        }

        public IActionResult Index(Guid? manufacturerId = null, int currentPage = 1)
        {
            Manufacturer? manufacturer = manufacturerId is null
                ? null
                : _manufacturerRepository.GetById(manufacturerId);

            var model = new HomeViewModel
            {
                Aircraft = _aircraftRepository
                    .FindAll(a => manufacturerId == null || a.ManufacturerId == manufacturerId)
                    .Include(a => a.Manufacturer)
                    .OrderBy(a => a.Id)
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = currentPage,
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
