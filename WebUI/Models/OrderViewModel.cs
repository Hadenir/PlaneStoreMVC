using System.ComponentModel.DataAnnotations;

namespace PlaneStore.WebUI.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter a street name")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Please enter a post code")]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public string? Country { get; set; }
    }
}
