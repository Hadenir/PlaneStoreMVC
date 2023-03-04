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
                    new Aircraft { Id = 1, Name = "A1" },
                    new Aircraft { Id = 2, Name = "A2" },
                    new Aircraft { Id = 3, Name = "A3" },
                    new Aircraft { Id = 4, Name = "A4" },
                    new Aircraft { Id = 5, Name = "A5" }
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
                new Aircraft { Id = 1, Name = "A1" },
                new Aircraft { Id = 2, Name = "A2" },
                new Aircraft { Id = 3, Name = "A3" },
                new Aircraft { Id = 4, Name = "A4" },
                new Aircraft { Id = 5, Name = "A5" }
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
