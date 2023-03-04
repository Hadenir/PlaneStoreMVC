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
        [Fact]
        public void Can_Paginate()
        {
            var mock = new Mock<IAircraftRepository>();
            mock.Setup(m => m.Aircraft)
                .Returns(new[] {
                    new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.Empty },
                    new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = Guid.Empty },
                    new Aircraft { Id = Guid.NewGuid(), Name = "A3", ManufacturerId = Guid.Empty },
                    new Aircraft { Id = Guid.NewGuid(), Name = "A4", ManufacturerId = Guid.Empty },
                    new Aircraft { Id = Guid.NewGuid(), Name = "A5", ManufacturerId = Guid.Empty }
                }.AsQueryable());

            var controller = new AircraftController(mock.Object)
            {
                PageSize = 3
            };

            var result = controller.List(2).Model as AircraftListViewModel;

            List<Aircraft> aircraft = result!.Aircraft.ToList();

            Assert.True(aircraft.Count == 2);
            Assert.Equal("A4", aircraft[0].Name);
            Assert.Equal("A5", aircraft[1].Name);
        }

        [Fact]
        public void Can_Send_Paginated_View_Model()
        {
            var mock = new Mock<IAircraftRepository>();
            mock.Setup(m => m.Aircraft)
                .Returns(new[] {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.Empty },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = Guid.Empty },
                new Aircraft { Id = Guid.NewGuid(), Name = "A3", ManufacturerId = Guid.Empty },
                new Aircraft { Id = Guid.NewGuid(), Name = "A4", ManufacturerId = Guid.Empty },
                new Aircraft { Id = Guid.NewGuid(), Name = "A5", ManufacturerId = Guid.Empty }
                }.AsQueryable());

            var controller = new AircraftController(mock.Object)
            {
                PageSize = 3
            };

            var result = controller.List(2).Model as AircraftListViewModel;

            PagingInfo pagingInfo = result!.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }
    }
}
