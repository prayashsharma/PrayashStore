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
            var cartItem = CartRepository.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

            if (cartItem == null)
            {
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
                cartItem.Count++;
                CartRepository.Edit(cartItem, cartItem.RecordId);
            }
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
                    Guid tempCartId = Guid.NewGuid();
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
                    CartRepository.Edit(productInCart, productInCart.RecordId);

                    Cart migratedCart = CartRepository.SingleOrDefault(x => x.CartId == item.CartId && x.ProductId == item.ProductId);
                    CartRepository.Remove(migratedCart);
                }
                else
                {
                    item.CartId = destinationCartId;
                    CartRepository.Edit(item, item.RecordId);
                }
            }
            UnitOfWork.Complete();

        }

        public int RemoveItemFromCart(int cartRecordId)
        {
            var cartItem = CartRepository.SingleOrDefault(x => x.CartId == ShoppingCartId && x.RecordId == cartRecordId);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    CartRepository.Edit(cartItem, cartItem.RecordId);
                    itemCount = cartItem.Count;
                }
                else
                {
                    CartRepository.Remove(cartItem);
                }
                UnitOfWork.Complete();
            }
            return itemCount;

        }

        public int RemoveMultipleItemsFromCart(int cartRecordId)
        {
            var cartItem = CartRepository.SingleOrDefault(x => x.CartId == ShoppingCartId && x.RecordId == cartRecordId);

            if (cartItem != null)
            {
                _cartRepository.Remove(cartItem);
                UnitOfWork.Complete();
            }
            return 0;
        }

        public Cart GetCartItemByRecordId(int recordId)
        {
            return CartRepository.Get(recordId);
        }
    }
}