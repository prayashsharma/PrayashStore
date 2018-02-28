using PrayashStore.Constants;
using PrayashStore.Extensions;
using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

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

        public void AddProductImage(Product product, HttpPostedFileBase image)
        {
            using (Image img = Image.FromStream(image.InputStream))
            {
                var data = img.Resize(ImageSizeConstant.ProductImageWidth, ImageSizeConstant.ProductImageHeight)
                                .ToByteArray(ImageFormat.Png);
                var productImage = new ProductImage
                {
                    ProductId = product.Id,
                    ImageData = data
                };
                ProductImageRepository.Add(productImage);
                UnitOfWork.Complete();
            }
        }

        public void RemoveProductImage(ProductImage productImage)
        {
            if (productImage != null)
            {
                ProductImageRepository.Remove(productImage);
                UnitOfWork.Complete();
            }
        }

        public IEnumerable<ProductImage> GetAllImagesByProductId(int productId)
        {
            return _productImageRepository.Find(x => x.ProductId == productId);
        }

        public ProductImage GetProductImageById(int productImageId)
        {
            return _productImageRepository.Get(productImageId);
        }
    }
}