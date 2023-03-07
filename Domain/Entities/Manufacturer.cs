namespace PlaneStore.Domain.Entities
{
    public class Manufacturer
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<Aircraft> ProducedAircraft { get; set; } = null!;
    }
}
