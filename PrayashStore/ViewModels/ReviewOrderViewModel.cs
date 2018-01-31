using PrayashStore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrayashStore.ViewModels
{
    public class ReviewOrderViewModel
    {
        public ReviewOrderViewModel()
        {
            BillingAddress = new AddressViewModel();
            ShippingAddress = new AddressViewModel();
        }
        public string CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public List<Cart> CartItems { get; set; }

        [Display(Name = "Promo Code")]
        public string PromoCode { get; set; }
        public AddressViewModel BillingAddress { get; set; }
        public AddressViewModel ShippingAddress { get; set; }

    }
}