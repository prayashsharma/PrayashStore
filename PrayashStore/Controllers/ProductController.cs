using PrayashStore.Constants;
using PrayashStore.Extensions;
using PrayashStore.Helpers;
using PrayashStore.Models;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize(Roles = "CanManageProducts,Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index(string categoryName, string gender)
        {
            ViewBag.Categories = _context.Categories.Select(x => x.Name).ToList();
            ViewBag.Genders = Enum.GetNames(typeof(Gender));

            var products = new List<Product>();

            if (string.IsNullOrWhiteSpace(categoryName) && string.IsNullOrWhiteSpace(gender))
            {
                products = _context.Products
                    .Include(c => c.Category)
                    .ToList();
                return View(products);
            }

            if (!string.IsNullOrWhiteSpace(categoryName) && !string.IsNullOrWhiteSpace(gender))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                products = _context.Products
                 .Include(c => c.Category)
                 .Where(x => x.Gender == genderEnum && x.Category.Name == categoryName)
                 .ToList();
                return View(products);
            }

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                products = _context.Products
                    .Include(c => c.Category)
                    .Where(x => x.Gender == genderEnum)
                    .ToList();
                return View(products);

            }
            if (string.IsNullOrWhiteSpace(gender))
            {
                products = _context.Products
                    .Include(c => c.Category)
                    .Where(x => x.Category.Name == categoryName)
                    .ToList();
                return View(products);
            }

            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _context.Products.Find(id);
            if (product == null)
                return HttpNotFound();


            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);

            var cartItem = cart.GetCartItems().Single(x => x.ProductId == product.Id);

            var productDetailViewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString(),
                Gender = product.Gender,
                Category = product.Category,
                ProductImages = _context.ProductImages.Where(x => x.ProductId == id).ToList(),
                CartItemCount = cart.GetItemCount(product.Id),
                CartItemRecordId = cartItem.RecordId
            };
            return View(productDetailViewModel);
        }

        public ActionResult Create()
        {
            var productAddFormViewModel = new ProductAddFormViewModel
            {
                Categories = _context.Categories.ToList()
            };
            return View(productAddFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductAddFormViewModel productAddFormViewModel)
        {

            if (!ModelState.IsValid)
            {
                productAddFormViewModel.Categories = _context.Categories.ToList();
                return View(productAddFormViewModel);
            }

            var product = new Product
            {
                Name = productAddFormViewModel.Name,
                Gender = productAddFormViewModel.Gender,
                Description = productAddFormViewModel.Description,
                Price = decimal.Parse(productAddFormViewModel.Price),
                CategoryId = productAddFormViewModel.Category
            };

            if (productAddFormViewModel.Thumbnail != null)
            {
                using (Image img = Image.FromStream(productAddFormViewModel.Thumbnail.InputStream))
                {
                    var data = img.Resize(ImageSizeConstant.ProductThumbnailWidth, ImageSizeConstant.ProductThumbnailHeight)
                                  .ToByteArray(ImageFormat.Jpeg);

                    product.Thumbnail = data;
                }
            }


            if (productAddFormViewModel.Images != null)
            {
                var imageList = new List<ProductImage>();
                foreach (var image in productAddFormViewModel.Images)
                {
                    if (image != null)
                    {
                        using (Image img = Image.FromStream(image.InputStream))
                        {
                            var data = img.Resize(ImageSizeConstant.ProductImageWidth, ImageSizeConstant.ProductImageHeight)
                                          .ToByteArray(ImageFormat.Jpeg);
                            var productImage = new ProductImage
                            {
                                ProductId = product.Id,
                                ImageData = data
                            };

                            imageList.Add(productImage);
                        }
                    }
                }
                product.ProductImages = imageList;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            Product product = _context.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            var productEditFormViewModel = new ProductEditFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.CategoryId,
                Description = product.Description,
                Price = product.Price.ToString(),
                Gender = product.Gender,
                Categories = _context.Categories.ToList(),
                ProductImages = _context.ProductImages.Where(x => x.ProductId == id).ToList()
            };
            return View(productEditFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditFormViewModel productEditFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                productEditFormViewModel.ProductImages = _context.ProductImages
                                                            .Where(x => x.ProductId == productEditFormViewModel.Id)
                                                            .ToList();
                productEditFormViewModel.Categories = _context.Categories.ToList();
                return View(productEditFormViewModel);
            }

            var product = _context.Products.Find(productEditFormViewModel.Id);

            product.Id = productEditFormViewModel.Id;
            product.Name = productEditFormViewModel.Name;
            product.Gender = productEditFormViewModel.Gender;
            product.Description = productEditFormViewModel.Description;
            product.Price = decimal.Parse(productEditFormViewModel.Price);
            product.CategoryId = productEditFormViewModel.Category;

            if (productEditFormViewModel.Thumbnail != null)
            {
                using (Image img = Image.FromStream(productEditFormViewModel.Thumbnail.InputStream))
                {
                    var data = img.Resize(ImageSizeConstant.ProductThumbnailWidth, ImageSizeConstant.ProductThumbnailHeight)
                                    .ToByteArray(ImageFormat.Png);

                    product.Thumbnail = data;
                }
            }

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
            if (product == null)
                return HttpNotFound();

            var productDetailViewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price.ToString(),
                Gender = product.Gender
            };
            return View(productDetailViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            if (!ModelState.IsValid)
                return View();

            var product = _context.Products.Find(id);

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
