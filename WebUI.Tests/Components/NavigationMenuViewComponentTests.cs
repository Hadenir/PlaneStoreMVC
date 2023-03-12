using Moq;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Components;
using PlaneStore.WebUI.Models.Components;
using Xunit;

namespace PlaneStore.WebUI.Tests.Components
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_List_Manufacturers()
        {
            var manufacturerService = new Mock<IManufacturerService>();
            manufacturerService.Setup(s => s.GetManufacturers())
                .Returns(new[]
                {
                    new Manufacturer { Name = "M1" },
                    new Manufacturer { Name = "M2" },
                    new Manufacturer { Name = "M3" },
                }.AsQueryable());

            var component = new NavigationMenuViewComponent(manufacturerService.Object);

            var result = component.Invoke().ViewData?.Model as NavigationMenuViewModel;

            Assert.True(Enumerable.SequenceEqual(new[] { "M1", "M2", "M3" }, result!.Manufacturers.Select(m => m.Name)));
        }

        [Fact]
        public void Indicates_Selected_Manufacturer()
        {
            var manufacturer = new Manufacturer { Id = Guid.NewGuid(), Name = "M1" };

            var manufacturerService = new Mock<IManufacturerService>();
            manufacturerService.Setup(s => s.GetManufacturerById(manufacturer.Id))
                .Returns(manufacturer);

            var component = new NavigationMenuViewComponent(manufacturerService.Object);

            var result = component.Invoke(manufacturer.Id).ViewData?.Model as NavigationMenuViewModel;

            Assert.Equal(manufacturer, result!.SelectedManufacturer);
        }
    }
}
