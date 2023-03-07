namespace PlaneStore.WebUI.Models
{
    public class CartViewModel
    {
        public Cart Cart { get; set; } = new();

        public string ReturnUrl { get; set; } = "/";
    }
}
