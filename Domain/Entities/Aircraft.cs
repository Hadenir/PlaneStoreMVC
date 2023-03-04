namespace PlaneStore.Domain.Entities
{
    public class Aircraft
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public required Guid ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; } = null!;
    }
}
