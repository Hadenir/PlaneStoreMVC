using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using Xunit;

namespace PlaneStore.Application.Tests.Models
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            var m = new Manufacturer { Id = Guid.NewGuid(), Name = "M1" };
            var a1 = new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = m };
            var a2 = new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = m };

            var cart = new Cart();

            cart.AddItem(a1, 1);
            cart.AddItem(a2, 1);

            Assert.Equal(2, cart.Lines.Count);
            Assert.Equal(a1, cart.Lines[0].Aircraft);
            Assert.Equal(a2, cart.Lines[1].Aircraft);
        }

        [Fact]
        public void Can_Add_Quantity_To_Existing_Lines()
        {
            var m = new Manufacturer { Name = "M1" };
            var a1 = new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = m };
            var a2 = new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = m };

            var cart = new Cart();

            cart.AddItem(a1, 1);
            cart.AddItem(a2, 2);
            cart.AddItem(a1, 10);

            Assert.Equal(2, cart.Lines.Count);
            Assert.Equal(a1, cart.Lines[0].Aircraft);
            Assert.Equal(11, cart.Lines[0].Quantity);
            Assert.Equal(a2, cart.Lines[1].Aircraft);
            Assert.Equal(2, cart.Lines[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            var m = new Manufacturer { Name = "M1" };
            var a1 = new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = m };
            var a2 = new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = m };
            var a3 = new Aircraft { Id = Guid.NewGuid(), Name = "A3", Manufacturer = m };

            var cart = new Cart();

            cart.AddItem(a1, 1);
            cart.AddItem(a2, 3);
            cart.AddItem(a3, 5);
            cart.AddItem(a2, 1);

            cart.RemoveItem(a2);

            Assert.Empty(cart.Lines.Where(l => l.Aircraft == a2));
            Assert.Equal(2, cart.Lines.Count);
        }

        [Fact]
        public void Can_Calculate_Total()
        {
            var m = new Manufacturer { Id = Guid.NewGuid(), Name = "M1" };
            var a1 = new Aircraft { Id = Guid.NewGuid(), Name = "A1", Price = 100_000, Manufacturer = m };
            var a2 = new Aircraft { Id = Guid.NewGuid(), Name = "A2", Price = 50_000, Manufacturer = m };

            var cart = new Cart();

            cart.AddItem(a1, 1);
            cart.AddItem(a2, 2);
            cart.AddItem(a1, 3);

            decimal total = cart.ComputeTotalPrice();

            Assert.Equal(4 * 100_000 + 2 * 50_000, total);
        }

        [Fact]
        public void Can_Clear_Items()
        {
            var m = new Manufacturer { Id = Guid.NewGuid(), Name = "M1" };
            var a1 = new Aircraft { Id = Guid.NewGuid(), Name = "A1", Manufacturer = m };
            var a2 = new Aircraft { Id = Guid.NewGuid(), Name = "A2", Manufacturer = m };

            var cart = new Cart();

            cart.AddItem(a1, 1);
            cart.AddItem(a2, 1);

            cart.Clear();

            Assert.Empty(cart.Lines);
        }
    }
}
