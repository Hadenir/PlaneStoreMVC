using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Services;
using PlaneStore.Application.Utilities;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    public class ManufacturersController : AdminControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public ManufacturersController(IManufacturerService manufacturerService, IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }

        public ViewResult Index()
        {
            var manufacturers = _manufacturerService.GetManufacturers().AsNoTracking();

            return View(_mapper.ProjectTo<ManufacturerViewModel>(manufacturers));
        }

        public IActionResult Details(Guid? id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
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
                    _manufacturerService.AddManufacturer(manufacturer);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        public IActionResult Edit(Guid? id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
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
            var manufacturer = _manufacturerService.GetManufacturerById(model.Id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _mapper.Map(model, manufacturer);
                    _manufacturerService.UpdateManufacturer(manufacturer);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        public IActionResult Remove(Guid? id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ManufacturerViewModel>(manufacturer));
        }

        [HttpPost, ActionName("Remove")]
        public IActionResult RemovePost(Guid? id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
            if (manufacturer is null)
            {
                return NotFound();
            }

            try
            {
                _manufacturerService.RemoveManufacturerById(manufacturer.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(_mapper.Map<ManufacturerViewModel>(manufacturer));
        }
    }
}
