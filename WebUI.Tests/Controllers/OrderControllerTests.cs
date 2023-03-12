using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlaneStore.Application.Models;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Utilities;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly IMapper _mapper;

        public OrderControllerTests()
        {
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            var orderService = new Mock<IOrderService>();

            var cart = new Cart();
            var orderModel = new OrderViewModel();

            var controller = new OrderController(cart, orderService.Object, _mapper);

            var result = controller.Checkout(orderModel) as ViewResult;

            orderService.Verify(s => s.PlaceOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            var orderService = new Mock<IOrderService>();

            var cart = new Cart();
            cart.AddItem(new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.NewGuid() }, 1);

            var orderModel = new OrderViewModel();
            var controller = new OrderController(cart, orderService.Object, _mapper);
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Checkout(orderModel) as ViewResult;

            orderService.Verify(s => s.PlaceOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            var orderService = new Mock<IOrderService>();

            var cart = new Cart();
            cart.AddItem(new Aircraft { Id = Guid.NewGuid(), Name = "A1", ManufacturerId = Guid.NewGuid() }, 1);

            var orderModel = new OrderViewModel();
            var controller = new OrderController(cart, orderService.Object, _mapper);

            var result = controller.Checkout(orderModel) as RedirectToActionResult;

            orderService.Verify(s => s.PlaceOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result?.ActionName);
        }
    }
}
