using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Serializable]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly IProductImageService _productImageService;

        public HomeController(IProductService productService,
                              ICategoryService categoryService, ICartService cartService,
                              IProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
            _productImageService = productImageService;
        }
        public ActionResult Index(int? id, string gender)
        {
            var products = new List<Product>();

            if (id == null && gender == null)
            {
                products = _productService.GetAllProducts().ToList();

            }
            else if (gender == null)
            {
                products = _productService.GetAllProductsByCategoryId(id.GetValueOrDefault()).ToList();
            }
            else if (id == null)
            {
                products = _productService.GetAllProductsByGender(gender).ToList();
            }
            else
            {
                products = _productService.GetAllProductsByCategoryIdAndGender(id.GetValueOrDefault(), gender).ToList();
            }

            ViewBag.GenderMenuItem = gender;
            ViewBag.CategoryMenuItem = (id == null) ? null : _categoryService.GetCategoryById(id.GetValueOrDefault()).Name;

            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return View("Error");

            Product product = _productService.GetProductById(id.GetValueOrDefault());
            if (product == null)
                return View("Error");

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
                CartItemRecordId = cartItem?.RecordId ?? 0
            };
            return View(productDetailViewModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}