namespace PrayashStore.ViewModels
{
    public class AddressUpdateViewModel
    {
        public AddressUpdateViewModel()
        {
            BillingAddress = new AddressViewModel();
            ShippingAddress = new AddressViewModel();
        }
        public AddressViewModel BillingAddress { get; set; }
        public AddressViewModel ShippingAddress { get; set; }
    }
}