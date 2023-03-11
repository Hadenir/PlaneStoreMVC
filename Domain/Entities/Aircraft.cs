using System.ComponentModel.DataAnnotations.Schema;

namespace PlaneStore.Domain.Entities
{
    public class Aircraft
    {
        public Guid Id { get; set; }
        
        public required string Name { get; set; }
        
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public required Guid ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}
