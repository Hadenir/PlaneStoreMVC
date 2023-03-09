using PlaneStore.Domain.Entities;

namespace PlaneStore.WebUI.Areas.Admin.Models
{
    public class OrdersViewModel
    {
        public required IEnumerable<Order> AllOrders { get; set; }
        public required IEnumerable<Order> UndeliveredOrders { get; set; }
        public required IEnumerable<Order> DeliveredOrders { get; set; }
    }
}
