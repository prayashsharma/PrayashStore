using PrayashStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayashStore.Helpers
{
    public class ShoppingCartHelper
    {
        private readonly ApplicationDbContext _context;
        //private ApplicationDbContext _context = ApplicationDbContext.Create();

        private string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public ShoppingCartHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        //public static ShoppingCartHelper GetCart(HttpContextBase context)
        //{
        //    var cart = new ShoppingCartHelper();
        //    cart.ShoppingCartId = cart.GetCartId(context);
        //    return cart;
        //}

        public static ShoppingCartHelper GetCart(ApplicationDbContext db, HttpContextBase context)
        {
            var cart = new ShoppingCartHelper(db);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        //public static ShoppingCartHelper GetCart(Controller controller)
        //{
        //    return GetCart(controller.HttpContext);
        //}
        //public static ShoppingCartHelper GetCart(ApplicationDbContext db, Controller controller)
        //{
        //    return GetCart(db, controller.HttpContext);
        //}


        public string CartId
        {
            get { return ShoppingCartId; }

        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and product instances
            var cartItem = _context.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == product.Id);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            _context.SaveChanges();
        }
        public int RemoveItemFromCart(int id)
        {
            // Get the cart
            var cartItem = _context.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _context.Carts.Remove(cartItem);
                }
                // Save changes
                _context.SaveChanges();
            }
            return itemCount;
        }

        public int RemoveMultipleItemsFromCart(int id)
        {
            // Get the cart
            var cartItem = _context.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }
            return 0;
        }
        public void EmptyCart()
        {
            var cartItems = _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _context.Carts.Remove(cartItem);
            }
            // Save changes
            _context.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _context.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public int GetItemCount(int productId)
        {
            var cartItem = _context.Carts.SingleOrDefault(x => x.CartId == ShoppingCartId && x.ProductId == productId);
            return cartItem != null ? cartItem.Count : 0;
        }
        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those products in the cart
            // sum all product price totals to get the cart total
            decimal? total = (from cartItems in _context.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        //We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            else if (!string.IsNullOrWhiteSpace(context.User.Identity.Name) && (context.User.Identity.Name != context.Session[CartSessionKey].ToString()))
            {
                MigrateCart(context.User.Identity.Name, context.Session[CartSessionKey].ToString());
                context.Session[CartSessionKey] = context.User.Identity.Name;
            }

            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string migrateTo, string migrateFrom)
        {
            var shoppingCart = _context.Carts
                .Where(c => c.CartId == migrateFrom)
                .ToList();

            foreach (Cart item in shoppingCart)
            {
                var productInCart = _context.Carts.SingleOrDefault(p => p.ProductId == item.ProductId && p.CartId == migrateTo);
                if (productInCart != null)
                {
                    productInCart.Count += item.Count;
                    Cart migratedCart = _context.Carts.Single(m => m.CartId == item.CartId && m.ProductId == item.ProductId);
                    _context.Carts.Remove(migratedCart);
                }
                else
                {
                    item.CartId = migrateTo;
                }
            }
            _context.SaveChanges();
        }

    }
}