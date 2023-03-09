using Microsoft.AspNetCore.Mvc;
using PlaneStore.Domain.Entities;
using PlaneStore.Domain.Repositories;
using PlaneStore.WebUI.Areas.Admin.Controllers;
using PlaneStore.WebUI.Tests.Mocks;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers.Admin
{
    public class OrdersControllerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            var orders = new[]
            {
                new Order { FullName = "N1", Street = "S1", City = "CY1", Country = "C1", PostCode = "PC1", IsDelivered = false },
                new Order { FullName = "N1", Street = "S1", City = "CY1", Country = "C1", PostCode = "PC1", IsDelivered = false },
            };

            _orderRepository = new OrderRepositoryMock(orders);
            _controller = new OrdersController(_orderRepository);
        }

        [Fact]
        public void Can_Mark_Order_Delivered()
        {
            var order = _orderRepository.GetAll().First();

            var result = _controller.Deliver(order.Id) as RedirectToActionResult;

            Assert.True(order.IsDelivered);
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void Can_Reset_Order_Delivered()
        {
            var order = _orderRepository.GetAll().First();

            var result = _controller.Reset(order.Id) as RedirectToActionResult;

            Assert.False(order.IsDelivered);
            Assert.Equal("Index", result?.ActionName);
        }
    }
}
