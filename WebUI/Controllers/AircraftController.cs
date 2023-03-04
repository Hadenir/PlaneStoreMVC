using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Models;

namespace PlaneStore.WebUI.Controllers
{
    public class AircraftController : Controller
    {
        public int PageSize = 4;

        private readonly IAircraftRepository _repository;

        public AircraftController(IAircraftRepository repository)
        {
            _repository = repository;
        }

        public ViewResult List(int page = 1)
        {
            var model = new AircraftListViewModel
            {
                Aircraft = _repository.Aircraft
                    .OrderBy(a => a.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Aircraft.Count(),
                },
            };

            return View(model);
        }
    }
}
