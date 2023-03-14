using System.ComponentModel.DataAnnotations;

namespace PlaneStore.WebUI.Areas.Identity.Models
{
    public class LoginViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter a user name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        public string? Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
