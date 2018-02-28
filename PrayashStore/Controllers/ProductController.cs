using PrayashStore.Constants;
using PrayashStore.Extensions;
using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
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
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly IProductImageService _productImageService;

        public ProductController(IProductService productService, ICategoryService categoryService,
            ICartService cartService, IProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
            _productImageService = productImageService;
        }
        public ActionResult Index(string categoryName, string gender)
        {
            ViewBag.Categories = _categoryService.GetAllCategories().Select(x => x.Name).ToList();
            ViewBag.Genders = Enum.GetNames(typeof(Gender));

            var products = new List<Product>();

            if (string.IsNullOrWhiteSpace(categoryName) && string.IsNullOrWhiteSpace(gender))
            {
                products = _productService.GetAllProducts().ToList();
                return View(products);
            }

            if (!string.IsNullOrWhiteSpace(categoryName) && !string.IsNullOrWhiteSpace(gender))
            {
                products = _productService.GetAllProductsByCategoryNameAndGender(categoryName, gender).ToList();
                return View(products);
            }

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                products = _productService.GetAllProductsByGender(gender).ToList();
                return View(products);
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                products = _productService.GetAllProductsByCategoryName(categoryName).ToList();
                return View(products);
            }

            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = _productService.GetProductById(id.GetValueOrDefault());
            if (product == null)
                return HttpNotFound();

            var cartItem = _cartService.GetCartItems().SingleOrDefault(x => x.ProductId == product.Id);

            var productDetailViewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString(),
                Gender = product.Gender,
                Category = product.Category,
                ProductImages = _productImageService.GetAllImagesByProductId(product.Id),
                CartItemCount = _cartService.GetItemCount(product.Id),
                CartItemRecordId = cartItem.RecordId
            };
            return View(productDetailViewModel);
        }

        public ActionResult Create()
        {
            var productAddFormViewModel = new ProductAddFormViewModel
            {
                Categories = _categoryService.GetAllCategories()
            };
            return View(productAddFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductAddFormViewModel productAddFormViewModel)
        {

            if (!ModelState.IsValid)
            {
                productAddFormViewModel.Categories = _categoryService.GetAllCategories();
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
            _productService.AddProduct(product);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = _productService.GetProductById(id.GetValueOrDefault());
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
                Categories = _categoryService.GetAllCategories(),
                ProductImages = _productImageService.GetAllImagesByProductId(id.GetValueOrDefault())
            };
            return View(productEditFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditFormViewModel productEditFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                productEditFormViewModel.ProductImages = _productImageService.GetAllImagesByProductId(productEditFormViewModel.Id);
                productEditFormViewModel.Categories = _categoryService.GetAllCategories();
                return View(productEditFormViewModel);
            }

            var product = _productService.GetProductById(productEditFormViewModel.Id);

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

            _productService.EditProduct(product);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = _productService.GetProductWithCategory(id.GetValueOrDefault());
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

            var product = _productService.GetProductById(id);
            _productService.RemoveProduct(product);
            return RedirectToAction("Index");
        }
    }
}
