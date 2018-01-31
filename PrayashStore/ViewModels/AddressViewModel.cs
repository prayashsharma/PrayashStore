using System.ComponentModel.DataAnnotations;

namespace PrayashStore.ViewModels
{
    public class AddressViewModel
    {
        [Display(Name = "Name")]
        public string Attn { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "Address Line 1")]
        public string Line1 { get; set; }

        [StringLength(70)]
        [Display(Name = "Address Line 2")]
        public string Line2 { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }
    }
}