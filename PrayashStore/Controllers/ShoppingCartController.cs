using PrayashStore.Attributes;
using PrayashStore.Helpers;
using PrayashStore.Models;
using PrayashStore.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                CartItems = cart.GetCartItems(),
                CartTotal = string.Format("{0:F}", cart.GetTotal()),
                CartCount = cart.GetCount()
            };

            return View(shoppingCartViewModel);
        }


        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var addedProduct = _context.Products.Single(product => product.Id == id);

            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);

            cart.AddToCart(addedProduct);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = addedProduct.Name + " has been added to your shopping cart",
                CartCount = cart.GetCount(),
                CartTotal = string.Format("{0:F}", cart.GetTotal())
            };

            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult UpdateMultipleQuantitiesToCart(int productId, int quantity, int cartRecordId)
        {
            var updatedProduct = _context.Products.Single(product => product.Id == productId);
            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);

            var updatedQuantity = quantity - cart.GetItemCount(productId);

            if (updatedQuantity > 0)
            {
                while (updatedQuantity > 0)
                {
                    cart.AddToCart(updatedProduct);
                    updatedQuantity--;
                }

            }

            if (updatedQuantity < 0)
            {
                while (updatedQuantity < 0)
                {
                    cart.RemoveItemFromCart(cartRecordId);
                    updatedQuantity++;
                }
            }

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = updatedProduct.Name + " has been updated in your shopping cart",
                CartCount = cart.GetCount(),
                CartTotal = string.Format("{0:F}", cart.GetTotal())
            };

            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult RemoveItemFromCart(int id)
        {
            var cart = ShoppingCartHelper.GetCart(_context, this.HttpContext);

            string productName = _context.Carts.Single(item => item.RecordId == id).Product.Name;

            int itemCount = cart.RemoveItemFromCart(id);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", cart.GetTotal()),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult RemoveMultipleItemsFromCart(int id)
        {
            var cart = ShoppingCartHelper.GetCart(_context, this.HttpContext);

            string productName = _context.Carts.Single(item => item.RecordId == id).Product.Name;

            int itemCount = cart.RemoveMultipleItemsFromCart(id);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", cart.GetTotal()),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult EmptyCart()
        {
            var cart = ShoppingCartHelper.GetCart(_context, this.HttpContext);
            cart.EmptyCart();

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = " All items has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", cart.GetTotal()),
                CartCount = cart.GetCount(),
                ItemCount = 0,
            };
            return Json(shoppingCartViewModel);
        }

        [ChildActionOnly]
        [NoCache]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCartHelper.GetCart(_context, this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("_CartSummary");
        }
    }

}