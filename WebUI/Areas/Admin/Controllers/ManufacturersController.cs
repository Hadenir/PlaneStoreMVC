using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Services;
using PlaneStore.Application.Utilities;
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

        public ViewResult Index()
        {
            var manufacturers = _manufacturerRepository.GetAll().AsNoTracking();

            return View(_mapper.ProjectTo<ManufacturerViewModel>(manufacturers));
        }

        public IActionResult Details(Guid? id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ManufacturerViewModel>(manufacturer));
        }

        public ViewResult Create() => View(new ManufacturerViewModel());

        [HttpPost]
        public IActionResult Create(ManufacturerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var manufacturer = _mapper.Map<Manufacturer>(model);
                    _manufacturerRepository.Add(manufacturer);
                    _manufacturerRepository.Commit();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }

            return View(model);
        }

        public IActionResult Edit(Guid? id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ManufacturerViewModel>(manufacturer);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ManufacturerViewModel model)
        {
            var manufacturer = _manufacturerRepository.GetById(model.Id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _mapper.Map(model, manufacturer);
                    _manufacturerRepository.Update(manufacturer);
                    _manufacturerRepository.Commit();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }

            return View(model);
        }

        public IActionResult Remove(Guid? id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ManufacturerViewModel>(manufacturer));
        }

        [HttpPost, ActionName("Remove")]
        public IActionResult RemovePost(Guid? id)
        {
            var manufacturer = _manufacturerRepository.GetById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            try
            {
                _manufacturerService.RemoveManufacturer(manufacturer.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }
            catch (OperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(_mapper.Map<ManufacturerViewModel>(manufacturer));
        }
    }
}
