using PrayashStore.Services.Interfaces;
using System.Web;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    public class ImageController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;

        public ImageController(IProductService productService, IProductImageService productImageService)
        {
            _productService = productService;
            _productImageService = productImageService;
        }

        public ActionResult GetProductThumbnail(int id)
        {
            var product = _productService.GetProductById(id);
            return File(product.Thumbnail, "image/png");
        }

        public ActionResult GetProductImage(int id)
        {
            var productImage = _productImageService.GetProductImageById(id);
            return File(productImage.ImageData, "image/png");
        }

        [HttpPost]
        public ActionResult AddProductImage(int productId, HttpPostedFileBase image)
        {
            if (image == null)
                return Json(new { success = false, message = "Image not found, Upload Failed" });

            var product = _productService.GetProductById(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found, Upload failed" });

            _productImageService.AddProductImage(product, image);
            return Json(new { success = true, message = "Image Uploaded Successfully" });
        }

        [HttpPost]
        public ActionResult DeleteProductImage(int id)
        {
            var productImage = _productImageService.GetProductImageById(id);
            if (productImage == null)
                return Json(new { success = false, message = "Image not found, Cannot Remove" });

            _productImageService.RemoveProductImage(productImage);
            return Json(new { success = true, message = "Image Removed Successfully" });
        }
    }
}
