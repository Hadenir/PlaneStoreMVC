using Microsoft.AspNetCore.Mvc;
using Moq;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class AircraftControllerTests
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IAircraftRepository _aircraftRepository;

        private readonly AircraftController _controller;

        public AircraftControllerTests()
        {
            var manufacturers = new[]
            {
                new Manufacturer { Id = Guid.NewGuid(), Name = "M1" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M2" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M3" },
            };

            var manufacturerRepository = new Mock<IManufacturerRepository>();
            manufacturerRepository.Setup(m => m.Manufacturers).Returns(manufacturers.OrderBy(m => m.Id).AsQueryable());
            _manufacturerRepository = manufacturerRepository.Object;

            var aircraft = new[] 
            {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A3", ManufacturerId = manufacturers[1].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A4", ManufacturerId = manufacturers[2].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A5", ManufacturerId = manufacturers[1].Id },
            };

            var aircraftRepository = new Mock<IAircraftRepository>();
            aircraftRepository.Setup(m => m.Aircraft).Returns(aircraft.OrderBy(a => a.Id).AsQueryable());
            _aircraftRepository = aircraftRepository.Object;

            _controller = new AircraftController(_aircraftRepository, _manufacturerRepository);
        }

        [Fact]
        public void Can_Paginate()
        {
            var aircraft = _aircraftRepository.Aircraft.ToList();
            _controller.PageSize = 3;

            var result = (_controller.List(page: 2) as ViewResult)?.Model as AircraftListViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(aircraft[3].Id, resultAircraft[0].Id);
            Assert.Equal(aircraft[4].Id, resultAircraft[1].Id);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            _controller.PageSize = 3;

            var result = (_controller.List(page: 2) as ViewResult)?.Model as AircraftListViewModel;

            PagingInfo pagingInfo = result!.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_By_Manufacturer()
        {
            var manufacturers = _manufacturerRepository.Manufacturers.ToList();
            _controller.PageSize = 3;

            var result = (_controller.List(manufacturers[1].Id) as ViewResult)?.Model as AircraftListViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(manufacturers[1].Id, resultAircraft[0].ManufacturerId);
            Assert.Equal(manufacturers[1].Id, resultAircraft[1].ManufacturerId);
        }
    }
}
