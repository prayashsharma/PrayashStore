using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.ViewModels
{
    public class ProductDetailViewModel
    {
        public ProductDetailViewModel()
        {
            ProductImages = new List<ProductImage>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public Gender Gender { get; set; }

        public Category Category { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }

        public int CartItemCount { get; set; }
        public int CartItemRecordId { get; set; }
    }
}