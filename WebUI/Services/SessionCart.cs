using Newtonsoft.Json;
using PlaneStore.Application.Models;
using PlaneStore.Domain.Entities;
using PlaneStore.WebUI.Utilities;

namespace PlaneStore.WebUI.Services
{
    public class SessionCart : Cart
    {
        private const string CartSessionKey = "Cart";

        [JsonIgnore]
        public ISession? Session { get; private set; }

        private SessionCart() { }

        public static SessionCart GetSessionCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            var sessionCart = session?.GetJson<SessionCart>(CartSessionKey) ?? new SessionCart();
            sessionCart.Session = session;
            return sessionCart;
        }        

        public override void AddItem(Aircraft aircraft, int quantity)
        {
            base.AddItem(aircraft, quantity);
            Session?.SetJson(CartSessionKey, this);
        }

        public override void RemoveItem(Aircraft aircraft)
        {
            base.RemoveItem(aircraft);
            Session?.SetJson(CartSessionKey, this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove(CartSessionKey);
        }
    }
}
