using System.ComponentModel.DataAnnotations;

namespace PlaneStore.WebUI.Areas.Admin.Models
{
    public class AircraftViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Please enter the aircraft name")]
        public string? Name { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a correct price")]
        public decimal Price { get; set; }

        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "Please select a manufacturer")]
        public Guid? ManufacturerId { get; set; }
    }
}
