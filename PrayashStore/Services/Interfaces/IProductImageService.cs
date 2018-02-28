using PrayashStore.Models;
using System.Collections.Generic;
using System.Web;

namespace PrayashStore.Services.Interfaces
{
    public interface IProductImageService
    {
        ProductImage GetProductImageById(int productImageId);
        IEnumerable<ProductImage> GetAllImagesByProductId(int productId);
        void AddProductImage(Product product, HttpPostedFileBase image);
        void RemoveProductImage(ProductImage productImage);
    }
}
