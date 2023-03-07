using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using System.Text;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class CartControllerTests
    {
        private IAircraftRepository _aircraftRepository;

        private CartController _controller;

        public CartControllerTests()
        {
            var manufacturer = new Manufacturer { Name = "M" };
            var aircraft = new[]
            {
                new Aircraft { Name = "A1", ManufacturerId = manufacturer.Id },
                new Aircraft { Name = "A2", ManufacturerId = manufacturer.Id },
            };

            _aircraftRepository = new AircraftRepositoryMock(aircraft);

            _controller = new CartController(_aircraftRepository);
        }

        [Fact]
        public void Can_Load_Cart()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();

            var cart = new Cart();
            cart.AddItem(aircraft[0], 2);
            cart.AddItem(aircraft[1], 1);

            var sessionMock = new Mock<ISession>();
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cart));
            sessionMock.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));

            var contextMock = new Mock<HttpContext>();
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object,
            };

            var resultModel = (_controller.Index("myUrl") as ViewResult)?.Model as CartViewModel;

            var resultCart = resultModel!.Cart;

            Assert.Equal(cart.Lines.Count, resultCart.Lines.Count);
            Assert.Equal("myUrl", resultModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            var aircraft = _aircraftRepository.GetAll().ToList();

            var cart = new Cart();

            var sessionMock = new Mock<ISession>();
            sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) =>
                {
                    cart = JsonConvert.DeserializeObject<Cart>(Encoding.UTF8.GetString(val));
                });

            var contextMock = new Mock<HttpContext>();
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object,
            };

            _controller.Add(aircraft[0].Id, "myUrl");

            Assert.Single(cart.Lines);
            Assert.Equal(aircraft[0].Id, cart.Lines[0].Aircraft.Id);
            Assert.Equal(1, cart.Lines[0].Quantity);
        }
    }
}
