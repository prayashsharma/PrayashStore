using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;

namespace PrayashStore.Services
{
    public class AddressService : BaseService, IAddressService
    {
        private readonly IRepository<Address> _addressRepository;

        public AddressService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _addressRepository = UnitOfWork.GetRepository<Address>();
        }

        public IRepository<Address> AddressRepository
        {
            get { return _addressRepository; }
        }

        public Address GetCustomerAddress(AddressType type, string userId)
        {
            return AddressRepository.SingleOrDefault(x => x.Type == type && x.ApplicationUserId == userId);
        }

        public void UpdateAddress(Address billingAddress, Address shippingAddress, string userId)
        {
            var currentShippingAddress = GetCustomerAddress(AddressType.Shipping, userId);
            var currentBillingAddress = GetCustomerAddress(AddressType.Billing, userId);

            if (currentShippingAddress == null)
            {
                AddressRepository.Add(shippingAddress);
            }
            else
            {
                shippingAddress.Id = currentShippingAddress.Id;
                AddressRepository.Edit(shippingAddress, currentShippingAddress.Id);
            }

            if (currentBillingAddress == null)
            {
                AddressRepository.Add(billingAddress);
            }
            else
            {
                billingAddress.Id = currentBillingAddress.Id;
                AddressRepository.Edit(billingAddress, currentBillingAddress.Id);
            }
            UnitOfWork.Complete();
        }

    }
}