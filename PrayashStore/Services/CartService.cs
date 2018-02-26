using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrayashStore.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        public string ShoppingCartId { get; private set; }
        public const string CartSessionKey = "CartId";

        public CartService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _cartRepository = UnitOfWork.GetRepository<Cart>();
            ShoppingCartId = GetCartId(new HttpContextWrapper(HttpContext.Current));
        }

        public IRepository<Cart> CartRepository
        {
            get { return _cartRepository; }
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and product instances
            var cartItem = CartRepository.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

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
                CartRepository.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            UnitOfWork.Complete();
        }

        public void EmptyCart()
        {
            var cartItems = CartRepository.Find(x => x.CartId == ShoppingCartId);
            CartRepository.RemoveRange(cartItems);
            UnitOfWork.Complete();
        }

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

        public List<Cart> GetCartItems()
        {
            return CartRepository.Find(x => x.CartId == ShoppingCartId) as List<Cart>;
        }

        public int GetCount()
        {
            int? count = GetCartItems().Sum(x => x.Count);
            return count ?? 0;
        }

        public int GetItemCount(int productId)
        {
            var cartItem = CartRepository
                .SingleOrDefault(x => x.CartId == ShoppingCartId && x.ProductId == productId);
            return cartItem != null ? cartItem.Count : 0;
        }

        public decimal GetTotal()
        {
            decimal? total = GetCartItems().Sum(x => x.Count * x.Product.Price);
            return total ?? decimal.Zero;
        }

        public void MigrateCart(string destinationCartId, string sourceCartId)
        {

            var shoppingCart = CartRepository.Find(x => x.CartId == sourceCartId).ToList();

            foreach (Cart item in shoppingCart)
            {
                var productInCart = CartRepository.SingleOrDefault(x => x.ProductId == item.ProductId && x.CartId == destinationCartId);
                if (productInCart != null)
                {
                    productInCart.Count += item.Count;
                    Cart migratedCart = CartRepository.SingleOrDefault(x => x.CartId == item.CartId && x.ProductId == item.ProductId);
                    CartRepository.Remove(migratedCart);
                }
                else
                {
                    item.CartId = destinationCartId;
                }
            }
            UnitOfWork.Complete();

        }

        public int RemoveItemFromCart(int cartRecordId)
        {
            throw new NotImplementedException();
        }

        public int RemoveMultipleItemsFromCart(int cartRecordId)
        {
            throw new NotImplementedException();
        }
    }
}