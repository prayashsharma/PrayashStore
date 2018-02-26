using PrayashStore.Models;
using System.Collections.Generic;
using System.Web;

namespace PrayashStore.Services.Interfaces
{
    public interface ICartService
    {
        string GetCartId(HttpContextBase context);
        void AddToCart(Product product);
        int RemoveItemFromCart(int cartRecordId);
        int RemoveMultipleItemsFromCart(int cartRecordId);
        void EmptyCart();
        List<Cart> GetCartItems();
        int GetCount(); //total count
        int GetItemCount(int productId);
        decimal GetTotal();
        Cart GetCartItemByRecordId(int recordId);
        void MigrateCart(string migrateTo, string migrateFrom);

    }
}
