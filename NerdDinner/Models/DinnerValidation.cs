using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NerdDinner.Models
{
    public class DinnerValidation
    {
        [HiddenInput(DisplayValue = false)]
        public int DinnerID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title may not be longer than 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(265, ErrorMessage = "Description may not be longer than 256 characters")]
        public string Description { get; set; }

        [StringLength(256, ErrorMessage = "Hosted By name may not be longer than 20 characters")]
        public string HostedBy { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, ErrorMessage = "Address may not be longer than 50 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(30, ErrorMessage = "Country may not be longer than 30 characters")]
        [UIHint("CountryDropDown")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Contact phone is required")]
        [StringLength(20, ErrorMessage = "Contact phone may not be longer than 20 characters")]
        public string ContactPhone { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Latitude { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Longitude { get; set; }
    }
}