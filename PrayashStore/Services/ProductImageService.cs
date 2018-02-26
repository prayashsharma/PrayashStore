using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System.Collections.Generic;

namespace PrayashStore.Services
{
    public class ProductImageService : BaseService, IProductImageService
    {
        private readonly IRepository<ProductImage> _productImageRepository;

        public ProductImageService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _productImageRepository = UnitOfWork.GetRepository<ProductImage>();
        }

        public IRepository<ProductImage> ProductImageRepository
        {
            get { return _productImageRepository; }
        }

        public IEnumerable<ProductImage> GetAllImagesByProductId(int productId)
        {
            return _productImageRepository.Find(x => x.ProductId == productId);
        }
    }
}