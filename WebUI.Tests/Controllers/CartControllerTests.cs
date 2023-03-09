using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class CartControllerTests
    {
        private IAircraftRepository _aircraftRepository;

        public CartControllerTests()
        {
            var manufacturer = new Manufacturer { Name = "M" };
            var aircraft = new[]
            {
                new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = manufacturer },
                new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = manufacturer },
            };

            _aircraftRepository = new AircraftRepositoryMock(aircraft);
        }

        [Fact]
        public void Can_Load_Cart()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();

            var cart = new Cart();
            cart.AddItem(aircraft[0], 2);
            cart.AddItem(aircraft[1], 1);

            var controller = new CartController(_aircraftRepository, cart);

            var resultModel = controller.Index("myUrl").Model as CartViewModel;

            var resultCart = resultModel!.Cart;

            Assert.Equal(2, resultCart.Lines.Count);
            Assert.Equal("myUrl", resultModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();

            var cart = new Cart();
            cart.AddItem(aircraft[1], 2);

            var controller = new CartController(_aircraftRepository, cart);

            controller.Add(aircraft[0].Id, "myUrl");
            controller.Remove(aircraft[1].Id, "myUrl");

            Assert.Single(cart.Lines);
            Assert.Equal(aircraft[0], cart.Lines[0].Aircraft);
            Assert.Equal(1, cart.Lines[0].Quantity);
        }
    }
}
