using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.Services.Interfaces
{
    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetAllImagesByProductId(int productId);
    }
}
