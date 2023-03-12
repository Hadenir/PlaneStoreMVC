using Microsoft.AspNetCore.Mvc;
using Moq;
using PlaneStore.Application.Services;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Areas.Admin.Controllers;
using Xunit;

namespace PlaneStore.WebUI.Tests.Controllers.Admin
{
    public class OrdersControllerTests
    {
        [Fact]
        public void Can_Mark_Order_Delivered()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                FullName = "N1",
                Street = "S1",
                City = "CY1",
                Country = "C1",
                PostCode = "PC1",
                IsDelivered = false,
            };

            var orderService = new Mock<IOrderService>();
            orderService.Setup(s => s.GetOrderById(order.Id))
                .Returns(order);

            var controller = new OrdersController(orderService.Object);

            var result = controller.Deliver(order.Id) as RedirectToActionResult;

            Assert.True(order.IsDelivered);
            Assert.Equal(nameof(OrdersController.Index), result?.ActionName);
        }

        [Fact]
        public void Can_Reset_Order_Delivered()
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                FullName = "N1",
                Street = "S1",
                City = "CY1",
                Country = "C1",
                PostCode = "PC1",
                IsDelivered = true,
            };

            var orderService = new Mock<IOrderService>();
            orderService.Setup(s => s.GetOrderById(order.Id))
                .Returns(order);

            var controller = new OrdersController(orderService.Object);

            var result = controller.Reset(order.Id) as RedirectToActionResult;

            Assert.False(order.IsDelivered);
            Assert.Equal(nameof(OrdersController.Index), result?.ActionName);
        }
    }
}
