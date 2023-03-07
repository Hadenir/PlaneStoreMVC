using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models.Components
{
    public class NavigationMenuViewModel
    {
        public Manufacturer? SelectedManufacturer { get; set; }

        public required IEnumerable<Manufacturer> Manufacturers { get; set; }
    }
}
