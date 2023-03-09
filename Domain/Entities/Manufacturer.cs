namespace PlaneStore.Domain.Entities
{
    public class Manufacturer
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Aircraft> ProducedAircraft { get; set; } = new List<Aircraft>();
    }
}
