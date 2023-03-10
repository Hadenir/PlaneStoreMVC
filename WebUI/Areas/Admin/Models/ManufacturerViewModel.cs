using System.ComponentModel.DataAnnotations;

namespace PlaneStore.WebUI.Areas.Admin.Models
{
    public class ManufacturerViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Please enter a name", AllowEmptyStrings = false)]
        public string? Name { get; set; }

        public ICollection<AircraftViewModel> ProducedAircraft { get; set; } = new List<AircraftViewModel>();
    }
}
