using AutoMapper;
using PrayashStore.Constants;
using PrayashStore.Extensions;
using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            var product = Mapper.Map<Product>(productAddFormViewModel);

            if (productAddFormViewModel.Thumbnail != null)
            {
                product.Thumbnail = ProcessImage("thumbnail", productAddFormViewModel.Thumbnail.InputStream,
                                                    ImageSizeConstant.ProductThumbnailHeight, ImageSizeConstant.ProductThumbnailWidth);
            }


            if (productAddFormViewModel.Images != null)
            {
                var imageList = new List<ProductImage>();
                foreach (var image in productAddFormViewModel.Images)
                {
                    if (image != null)
                    {
                        var productImage = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageData = ProcessImage("image", image.InputStream,
                                                        ImageSizeConstant.ProductImageHeight, ImageSizeConstant.ProductImageWidth)
                        };

                        imageList.Add(productImage);
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

            var productEditFormViewModel = Mapper.Map<ProductEditFormViewModel>(product);
            productEditFormViewModel.Categories = _categoryService.GetAllCategories();
            productEditFormViewModel.ProductImages = _productImageService.GetAllImagesByProductId(id.GetValueOrDefault());

            return View(productEditFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditFormViewModel productEditFormViewModel)
        {
            var originalProduct = _productService.GetProductById(productEditFormViewModel.Id);
            if (!ModelState.IsValid || originalProduct == null)
            {
                productEditFormViewModel.ProductImages = _productImageService.GetAllImagesByProductId(productEditFormViewModel.Id);
                productEditFormViewModel.Categories = _categoryService.GetAllCategories();
                return View(productEditFormViewModel);
            }

            var product = Mapper.Map<Product>(productEditFormViewModel);

            if (productEditFormViewModel.Thumbnail != null)
            {
                product.Thumbnail = ProcessImage("thumbnail", productEditFormViewModel.Thumbnail.InputStream,
                                                    ImageSizeConstant.ProductThumbnailHeight, ImageSizeConstant.ProductThumbnailWidth);
            }
            else
            {
                product.Thumbnail = originalProduct.Thumbnail;
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

            var productDetailViewModel = Mapper.Map<ProductDetailViewModel>(product);
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

        private byte[] ProcessImage(string imageType, Stream imageStream, int height = 250, int width = 250)
        {
            using (Image img = Image.FromStream(imageStream))
            {
                var data = img.Resize(width, height)
                                .ToByteArray(ImageFormat.Png);
                return data;
            }

        }

    }
}
