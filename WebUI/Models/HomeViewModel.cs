using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models
{
    public class HomeViewModel
    {
        public required IEnumerable<Aircraft> Aircraft { get; set; }

        public PagingInfo PagingInfo { get; set; } = new();

        public Manufacturer? SelectedManufacturer { get; set; }
    }
}
