using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models
{
    public class AircraftListViewModel
    {
        public required IEnumerable<Aircraft> Aircraft { get; set; }
        public required PagingInfo PagingInfo { get; set; }
        public Manufacturer? SelectedManufacturer { get; set; }
    }
}
