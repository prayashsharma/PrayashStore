using PrayashStore.Constants;
using PrayashStore.Extensions;
using PrayashStore.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Image
        public ActionResult GetProductThumbnail(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            byte[] image;
            if (product.Thumbnail == null)
            {
                string filePath = Server.MapPath(Url.Content("~/Catalog/thumbnails/No-image-available.jpg"));
                image = System.IO.File.ReadAllBytes(filePath);
            }
            else
            {
                image = product.Thumbnail;
            }

            return File(image, "image/png");
        }

        public ActionResult GetProductImage(int id)
        {
            var productImage = _context.ProductImages.FirstOrDefault(x => x.Id == id);

            byte[] image;
            if (productImage.ImageData == null)
            {
                string filePath = Server.MapPath(Url.Content("~/Catalog/thumbnails/No-image-available.jpg"));
                image = System.IO.File.ReadAllBytes(filePath);
            }
            else
            {
                image = productImage.ImageData;
            }

            return File(image, "image/png");
        }

        [HttpPost]
        public ActionResult AddProductImage(int productId, HttpPostedFileBase image)
        {

            if (image == null)
                return Json(new { success = false, message = "Image not found, Upload Failed" });

            Product product = _context.Products.Find(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found, Upload failed" });

            using (Image img = Image.FromStream(image.InputStream))
            {
                var data = img.Resize(ImageSizeConstant.ProductImageWidth, ImageSizeConstant.ProductImageHeight)
                                .ToByteArray(ImageFormat.Png);
                var productImage = new ProductImage
                {
                    ProductId = product.Id,
                    ImageData = data
                };
                _context.ProductImages.Add(productImage);
                _context.SaveChanges();
                return Json(new { success = true, message = "Image Uploaded Successfully" });
            }
        }

        [HttpPost]
        public ActionResult DeleteProductImage(int id)
        {
            var productImage = _context.ProductImages.Find(id);
            if (productImage == null)
                return Json(new { success = false, message = "Image not found, Cannot Remove" });

            _context.ProductImages.Remove(productImage);
            _context.SaveChanges();
            return Json(new { success = true, message = "Image Removed Successfully" });
        }
    }
}
