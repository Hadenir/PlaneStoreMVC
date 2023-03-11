using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AircraftController : Controller
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public AircraftController(
            IAircraftRepository aircraftRepository,
            IManufacturerRepository manufacturerRepository,
            IMapper mapper)
        {
            _aircraftRepository = aircraftRepository;
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
        }

        public ViewResult Index()
        {
            var aircraft = _aircraftRepository
                .GetAll()
                .Include(a => a.Manufacturer)
                .AsNoTracking();

            return View(_mapper.ProjectTo<AircraftViewModel>(aircraft));
        }

        public IActionResult Details(Guid? id)
        {
            var aircraft = _aircraftRepository.GetById(id);
            if (aircraft is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AircraftViewModel>(aircraft));
        }

        public ViewResult Create()
        {
            ViewBag.Manufacturers = _manufacturerRepository
                .GetAll()
                .AsNoTracking()
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });
            return View(new AircraftViewModel());
        }

        [HttpPost]
        public IActionResult Create(AircraftViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var aircraft = _mapper.Map<Aircraft>(model);
                    _aircraftService.AddAircraft(aircraft);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes to database.");
            }

            ViewBag.Manufacturers = _manufacturerRepository
                .GetAll()
                .AsNoTracking()
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });
            return View(model);
        }

        //public IActionResult Edit(Guid id?)
        //{

        //}

        //[HttpPost]
        //public IActionResult Edit(AircraftViewModel model)
        //{

        //}

        //public IActionResult Remove(Guid? id)
        //{

        //}

        //[HttpPost, ActionName("Remove")]
        //public IActionResult RemovePost(Guid? id)
        //{

        //}
    }
}
