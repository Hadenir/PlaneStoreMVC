namespace PlaneStore.Domain.Entities
{
    public class OrderLine
    {
        public Guid Id { get; set; }
        public required Aircraft Aircraft { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class Order
    {
        public Guid Id { get; set; }

        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();

        public required string FullName { get; set; }

        public required string Street { get; set; }

        public required string City { get; set; }

        public required string PostCode { get; set; }

        public required string Country { get; set; }
    }
}
