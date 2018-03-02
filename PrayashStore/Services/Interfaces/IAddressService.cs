using PrayashStore.Models;

namespace PrayashStore.Services.Interfaces
{
    public interface IAddressService
    {
        Address GetCustomerAddress(AddressType type, string userId);
        void UpdateAddress(Address billingAddress, Address shippingAddress, string userId);
    }
}
