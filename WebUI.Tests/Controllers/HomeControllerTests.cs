using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var manufacturers = new[]
            {
                new Manufacturer { Id = Guid.NewGuid(), Name = "M1" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M2" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M3" },
            };

            _manufacturerRepository = new ManufacturerRepositoryMock(manufacturers);

            var aircraft = new[]
            {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = manufacturers[0] },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = manufacturers[0] },
                new Aircraft { Id = Guid.NewGuid(), Name = "A3", Manufacturer = manufacturers[1] },
                new Aircraft { Id = Guid.NewGuid(), Name = "A4", Manufacturer = manufacturers[2] },
                new Aircraft { Id = Guid.NewGuid(), Name = "A5", Manufacturer = manufacturers[1] },
            };

            _aircraftRepository = new AircraftRepositoryMock(aircraft);

            _controller = new HomeController(_aircraftRepository, _manufacturerRepository);
        }

        [Fact]
        public void Can_Paginate()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();
            _controller.PageSize = 3;

            var result = _controller.Index(currentPage: 2).Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            _controller.PageSize = 3;

            var result = _controller.Index(currentPage: 2).Model as HomeViewModel;

            PagingInfo pagingInfo = result!.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_By_Manufacturer()
        {
            var manufacturers = _manufacturerRepository.GetAll().ToList();
            _controller.PageSize = 3;

            var result = _controller.Index(manufacturers[1].Id).Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(manufacturers[1], resultAircraft[0].Manufacturer);
            Assert.Equal(manufacturers[1], resultAircraft[1].Manufacturer);
        }
    }
}
