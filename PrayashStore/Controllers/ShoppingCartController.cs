using PrayashStore.Attributes;
using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public ShoppingCartController(ApplicationDbContext context, ICartService cartService, IProductService productService)
        {
            _context = context;
            _cartService = cartService;
            _productService = productService;
        }

        public ActionResult Index()
        {
            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                CartItems = _cartService.GetCartItems(),
                CartTotal = string.Format("{0:F}", _cartService.GetTotal()),
                CartCount = _cartService.GetCount()
            };

            return View(shoppingCartViewModel);
        }


        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var addedProduct = _productService.GetProductById(id);
            _cartService.AddToCart(addedProduct);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = addedProduct.Name + " has been added to your shopping cart",
                CartCount = _cartService.GetCount(),
                CartTotal = string.Format("{0:F}", _cartService.GetTotal())
            };

            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult UpdateMultipleQuantitiesToCart(int productId, int quantity, int cartRecordId)
        {
            var updatedProduct = _productService.GetProductById(productId);
            var updatedQuantity = quantity - _cartService.GetItemCount(productId);

            if (updatedQuantity > 0)
            {
                while (updatedQuantity > 0)
                {
                    _cartService.AddToCart(updatedProduct);
                    updatedQuantity--;
                }

            }

            if (updatedQuantity < 0)
            {
                while (updatedQuantity < 0)
                {
                    _cartService.RemoveItemFromCart(cartRecordId);
                    updatedQuantity++;
                }
            }

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = updatedProduct.Name + " has been updated in your shopping cart",
                CartCount = _cartService.GetCount(),
                CartTotal = string.Format("{0:F}", _cartService.GetTotal())

            };

            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult RemoveItemFromCart(int id)
        {
            string productName = _cartService.GetCartItemByRecordId(id).Product.Name;
            int itemCount = _cartService.RemoveItemFromCart(id);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", _cartService.GetTotal()),
                CartCount = _cartService.GetCount(),

                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult RemoveMultipleItemsFromCart(int id)
        {
            string productName = _cartService.GetCartItemByRecordId(id).Product.Name;
            int itemCount = _cartService.RemoveMultipleItemsFromCart(id);

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", _cartService.GetTotal()),
                CartCount = _cartService.GetCount(),

                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(shoppingCartViewModel);
        }

        [HttpPost]
        public ActionResult EmptyCart()
        {
            _cartService.EmptyCart();

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                Success = true,
                Message = " All items has been removed from your shopping cart.",
                CartTotal = string.Format("{0:F}", _cartService.GetTotal()),
                CartCount = _cartService.GetCount(),

                ItemCount = 0,
            };
            return Json(shoppingCartViewModel);
        }

        [ChildActionOnly]
        [NoCache]
        public ActionResult CartSummary()
        {
            ViewData["CartCount"] = _cartService.GetCount();
            return PartialView("_CartSummary");
        }
    }

}