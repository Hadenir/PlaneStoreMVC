using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models
{
    public class AircraftListViewModel
    {
        public IEnumerable<Aircraft> Aircraft { get; set; } = Enumerable.Empty<Aircraft>();
        public required PagingInfo PagingInfo { get; set; }
        public Manufacturer? SelectedManufacturer { get; set; }
    }
}
