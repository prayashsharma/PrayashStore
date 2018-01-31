using System.ComponentModel.DataAnnotations;

namespace PrayashStore.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public AddressType Type { get; set; }

        [StringLength(70)]
        public string Attn { get; set; }

        [Required]
        [StringLength(70)]
        public string Line1 { get; set; }

        [StringLength(70)]
        public string Line2 { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}