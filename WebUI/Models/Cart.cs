using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models
{
    public class CartLine
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Aircraft Aircraft { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public List<CartLine> Lines { get; private set; } = new();

        public void AddItem(Aircraft aircraft, int quantity)
        {
            CartLine? cartLine = Lines
                .Where(a => a.Aircraft.Id == aircraft.Id)
                .FirstOrDefault();

            if (cartLine is null)
            {
                Lines.Add(new CartLine
                {
                    Aircraft = aircraft,
                    Quantity = quantity,
                });
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public void RemoveItem(Aircraft aircraft) => Lines.RemoveAll(l => l.Aircraft.Id == aircraft.Id);

        public decimal ComputeTotalPrice() => Lines.Sum(l => l.Quantity * l.Aircraft.Price);

        public void Clear() => Lines.Clear();
    }
}
