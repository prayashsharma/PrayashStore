using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsByCategoryId(int id);
        IEnumerable<Product> GetAllProductsByGender(string gender);
        IEnumerable<Product> GetAllProductsByCategoryIdAndGender(int id, string gender);
        Product GetProductById(int id);
    }
}
