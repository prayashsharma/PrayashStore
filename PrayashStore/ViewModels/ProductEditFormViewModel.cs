using PrayashStore.Attributes;
using PrayashStore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PrayashStore.ViewModels
{
    public class ProductEditFormViewModel
    {
        public ProductEditFormViewModel()
        {
            Categories = new List<Category>();
            Images = new List<HttpPostedFileBase>();
            ProductImages = new List<ProductImage>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"^\d{1,20}(\.\d{1,2})?$", ErrorMessage = "Price is not a valid currency.")]
        public string Price { get; set; }

        [EnumDataType(typeof(Gender), ErrorMessage = "Select a gender.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Select a category.")]
        public int Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        [FileType("JPG,JPEG,PNG")]
        [FileSize(1000000)]
        public HttpPostedFileBase Thumbnail { get; set; }

        [FileType("JPG,JPEG,PNG")]
        [FileSize(1000000)]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }
    }
}