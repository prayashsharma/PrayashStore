using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductWithCategory(int productId);
        IEnumerable<Product> GetAllProductsByCategoryId(int categoryId);
        IEnumerable<Product> GetAllProductsByCategoryName(string categoryName);
        IEnumerable<Product> GetAllProductsByGender(string gender);
        IEnumerable<Product> GetAllProductsByCategoryIdAndGender(int categoryId, string gender);
        IEnumerable<Product> GetAllProductsByCategoryNameAndGender(string categoryName, string gender);
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void EditProduct(Product product);
        void RemoveProduct(Product product);

    }
}
