using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
        public List<Cart> CartItems { get; set; }
    }
}