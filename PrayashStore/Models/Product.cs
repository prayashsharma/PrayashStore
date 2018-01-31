using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrayashStore.Models
{
    public class Product
    {
        public Product()
        {
            ProductImages = new List<ProductImage>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public byte[] Thumbnail { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}