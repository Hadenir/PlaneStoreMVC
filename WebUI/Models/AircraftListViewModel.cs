using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Models
{
    public class AircraftListViewModel
    {
        public IEnumerable<Aircraft> Aircraft { get; set; } = Enumerable.Empty<Aircraft>();
        public PagingInfo PagingInfo { get; set; } = null!;
    }
}
