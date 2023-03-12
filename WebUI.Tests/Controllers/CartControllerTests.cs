using Moq;
using PlaneStore.Application.Models;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class CartControllerTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            var aircraft = new[] {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.NewGuid() },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = Guid.NewGuid() },
            };

            var aircraftService = new Mock<IAircraftService>();

            var cart = new Cart();
            cart.AddItem(aircraft[0], 2);
            cart.AddItem(aircraft[1], 1);

            var controller = new CartController(aircraftService.Object, cart);

            var resultModel = controller.Index("myUrl").Model as CartViewModel;

            var resultCart = resultModel!.Cart;

            Assert.Equal(2, resultCart.Lines.Count);
            Assert.True(Enumerable.SequenceEqual(aircraft, resultCart.Lines.Select(l => l.Aircraft)));
            Assert.Equal("myUrl", resultModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            var aircraft = new[] {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.NewGuid() },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", ManufacturerId = Guid.NewGuid() },
            };

            var aircraftService = new Mock<IAircraftService>();
            aircraftService.Setup(s => s.GetAircraftById(It.IsAny<Guid>()))
                .Returns<Guid>(id => aircraft.First(a => a.Id == id));

            var cart = new Cart();
            cart.AddItem(aircraft[1], 2);

            var controller = new CartController(aircraftService.Object, cart);

            controller.Add(aircraft[0].Id, "myUrl");
            controller.Remove(aircraft[1].Id, "myUrl");

            Assert.Single(cart.Lines);
            Assert.Equal(aircraft[0], cart.Lines[0].Aircraft);
            Assert.Equal(1, cart.Lines[0].Quantity);
        }
    }
}
