using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Controllers;
using PlaneStore.WebUI.Models;
using PlaneStore.WebUI.Tests.Mocks;
using PlaneStore.WebUI.Utilities;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderControllerTests()
        {
            _orderRepository = new OrderRepositoryMock(Enumerable.Empty<Order>());

            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            var cart = new Cart();
            var orderModel = new OrderViewModel();

            var controller = new OrderController(cart, _orderRepository, _mapper);

            var result = controller.Checkout(orderModel) as ViewResult;

            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            var cart = new Cart();
            cart.AddItem(new Aircraft { Name = "A1", Manufacturer = new Manufacturer { Name = "M1" } }, 1);

            var orderModel = new OrderViewModel();
            var controller = new OrderController(cart, _orderRepository, _mapper);
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Checkout(orderModel) as ViewResult;

            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            var cart = new Cart();
            cart.AddItem(new Aircraft { Name = "A1", Manufacturer = new Manufacturer { Name = "M1" } }, 1);

            var orderModel = new OrderViewModel();
            var controller = new OrderController(cart, _orderRepository, _mapper);

            var result = controller.Checkout(orderModel) as RedirectToActionResult;

            Assert.Single(_orderRepository.GetAll());
            Assert.Equal("Completed", result?.ActionName);
        }
    }
}
