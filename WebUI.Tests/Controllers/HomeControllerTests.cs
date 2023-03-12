using Moq;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Manufacturer[] _manufacturers;
        private readonly Aircraft[] _aircraft;

        public HomeControllerTests()
        {
            _manufacturers = new[]
            {
                new Manufacturer { Id = Guid.NewGuid(), Name = "M1" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M2" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M3" },
            };

            _aircraft = new[]
            {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = _manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = _manufacturers[0].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A3", ManufacturerId = _manufacturers[1].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A4", ManufacturerId = _manufacturers[2].Id },
                new Aircraft { Id = Guid.NewGuid(), Name = "A5", ManufacturerId = _manufacturers[1].Id },
            };
        }

        [Fact]
        public void Can_Paginate()
        {
            var aircraftService = new Mock<IAircraftService>();
            aircraftService.Setup(s => s.GetAircraft()).Returns(_aircraft.AsQueryable());

            var manufacturerService = new Mock<IManufacturerService>();

            var controller = new HomeController(aircraftService.Object, manufacturerService.Object)
            {
                PageSize = 3,
            };

            var result = controller.Index(currentPage: 2).Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            var aircraftService = new Mock<IAircraftService>();
            aircraftService.Setup(s => s.GetAircraft()).Returns(_aircraft.AsQueryable());

            var manufacturerService = new Mock<IManufacturerService>();

            var controller = new HomeController(aircraftService.Object, manufacturerService.Object)
            {
                PageSize = 3,
            };

            var result = controller.Index(currentPage: 2).Model as HomeViewModel;

            PagingInfo pagingInfo = result!.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_By_Manufacturer()
        {
            var aircraftService = new Mock<IAircraftService>();
            aircraftService.Setup(s => s.GetAircraft()).Returns(_aircraft.AsQueryable());

            var manufacturerService = new Mock<IManufacturerService>();
            manufacturerService.Setup(s => s.GetManufacturerById(_manufacturers[0].Id))
                .Returns(_manufacturers[0]);

            var controller = new HomeController(aircraftService.Object, manufacturerService.Object)
            {
                PageSize = 3,
            };

            var result = controller.Index(_manufacturers[0].Id).Model as HomeViewModel;

            List<Aircraft> resultAircraft = result!.Aircraft.ToList();

            Assert.Equal(2, resultAircraft.Count);
            Assert.Equal(_manufacturers[0].Id, resultAircraft[0].ManufacturerId);
            Assert.Equal(_manufacturers[0].Id, resultAircraft[1].ManufacturerId);
        }
    }
}
