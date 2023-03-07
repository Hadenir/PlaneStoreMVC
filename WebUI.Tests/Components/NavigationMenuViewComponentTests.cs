using Microsoft.AspNetCore.Mvc.ViewComponents;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Components;
using PlaneStore.WebUI.Models.Components;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Components
{
    public class NavigationMenuViewComponentTests
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        private readonly NavigationMenuViewComponent _component;

        public NavigationMenuViewComponentTests()
        {
            var manufacturers = new[]
            {
                new Manufacturer { Id = Guid.NewGuid(), Name = "M3" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M2" },
                new Manufacturer { Id = Guid.NewGuid(), Name = "M1" },
            }.OrderBy(m => m.Id).ToList();

            _manufacturerRepository = new ManufacturerRepositoryMock(manufacturers);

            _component = new NavigationMenuViewComponent(_manufacturerRepository);
        }

        [Fact]
        public void Can_List_Manufacturers()
        {
            var result = ((NavigationMenuViewModel)((ViewViewComponentResult)_component.Invoke()).ViewData!.Model!);

            Assert.True(Enumerable.SequenceEqual(new[] { "M1", "M2", "M3" }, result.Manufacturers.Select(m => m.Name)));
        }

        [Fact]
        public void Indicates_Selected_Manufacturer()
        {
            var selectedManufacturer = _manufacturerRepository.GetAll().First();

            var result = ((NavigationMenuViewModel)((ViewViewComponentResult)_component.Invoke(selectedManufacturer.Id)).ViewData!.Model!);

            Assert.Equal(selectedManufacturer, result.SelectedManufacturer);
        }
    }
}
