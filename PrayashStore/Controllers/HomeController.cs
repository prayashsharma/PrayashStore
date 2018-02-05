using PrayashStore.Helpers;
using PrayashStore.Models;
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
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index(int? id, string gender)
        {
            var products = new List<Product>();

            if (id == null && gender == null)
            {
                products = _context.Products.OrderBy(c => c.CategoryId).ToList();
            }
            else if (gender == null)
            {
                products = _context.Products.Where(p => p.CategoryId == id).ToList();
            }
            else if (id == null)
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                products = _context.Products.Where(g => g.Gender == genderEnum).ToList();
            }
            else
            {
                Gender genderEnum = (Gender)Enum.Parse(typeof(Gender), gender);
                products = _context.Products.Where(p => p.CategoryId == id && p.Gender == genderEnum).ToList();
            }

            ViewBag.GenderMenuItem = gender;
            ViewBag.CategoryMenuItem = (id == null) ? null : _context.Categories.SingleOrDefault(x => x.Id == id).Name;

            return View(products);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return View("Error");

            Product product = _context.Products.Find(id);
            if (product == null)
                return View("Error");

            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);
            var cartItem = cart.GetCartItems().SingleOrDefault(x => x.ProductId == product.Id);
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
                CartItemRecordId = cartItem?.RecordId ?? 0
            };
            return View(productDetailViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}