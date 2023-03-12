using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Application.Services;
using PlaneStore.Application.Utilities;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Areas.Admin.Models;

namespace PlaneStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AircraftController : Controller
    {
        private readonly IAircraftService _aircraftService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public AircraftController(
            IAircraftService aircraftService,
            IManufacturerService manufacturerService,
            IMapper mapper)
        {
            _aircraftService = aircraftService;
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }

        public ViewResult Index()
        {
            var aircraft = _aircraftService.GetAircraft().AsNoTracking();

            return View(_mapper.ProjectTo<AircraftViewModel>(aircraft));
        }

        public IActionResult Details(Guid? id)
        {
            var aircraft = _aircraftService.GetAircraftById(id);
            if (aircraft is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AircraftViewModel>(aircraft));
        }

        public ViewResult Create()
        {
            ViewBag.Manufacturers = PrepareManufacturersList();
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
            catch (ServiceException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Manufacturers = PrepareManufacturersList();
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

        private IEnumerable<SelectListItem> PrepareManufacturersList()
            => _manufacturerService.GetManufacturers()
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name });
    }
}
