using Newtonsoft.Json;

namespace PlaneStore.Domain.Entities
{
    public class Manufacturer
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        [JsonIgnore]
        public ICollection<Aircraft> ProducedAircraft { get; set; } = new List<Aircraft>();
    }
}
