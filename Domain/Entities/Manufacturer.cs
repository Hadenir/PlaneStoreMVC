namespace PlaneStore.Domain.Entities
{
    public class Manufacturer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }
    }
}
