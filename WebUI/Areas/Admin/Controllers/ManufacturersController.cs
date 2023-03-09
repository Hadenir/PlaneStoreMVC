using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManufacturersController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public ManufacturersController(IManufacturerService manufacturerService, IManufacturerRepository manufacturerRepository, IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
        }

        public ViewResult Index() => View(_manufacturerRepository.GetAll());

        public IActionResult Details(Guid id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
                return NotFound();

            return View(manufacturer);
        }

        public ViewResult Create() => View("Edit", new ManufacturerViewModel());

        public IActionResult Edit(Guid id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
                return NotFound();

            var viewModel = _mapper.Map<ManufacturerViewModel>(manufacturer);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(ManufacturerViewModel model)
        {
            var manufacturer = _manufacturerRepository.GetById(model.Id);
            if (manufacturer is null)
            {
                manufacturer = _mapper.Map<Manufacturer>(model);
            }
            else
            {
                _mapper.Map(model, manufacturer);
            }

            _manufacturerRepository.Update(manufacturer);
            _manufacturerRepository.Commit();

            return RedirectToAction("Index");
        }

        public ViewResult Remove(Guid id) => View(_manufacturerRepository.GetById(id));

        [HttpPost]
        [ActionName("Remove")]
        public IActionResult RemovePost(Guid id)
        {
            if(!_manufacturerService.RemoveManufacturer(id))
            {
                return BadRequest("Could not remove manufacturer");
            }

            return RedirectToAction("Index");
        }
    }
}
