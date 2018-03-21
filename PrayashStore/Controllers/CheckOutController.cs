using AutoMapper;
using Microsoft.AspNet.Identity;
using PrayashStore.Constants;
using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CheckOutController(IAddressService addressService, ICartService cartService, IOrderService orderService)
        {
            _addressService = addressService;
            _cartService = cartService;
            _orderService = orderService;
        }
        public ActionResult Address()
        {
            var applicationUserId = User.Identity.GetUserId();
            var currentShippingAddress = _addressService.GetCustomerAddress(AddressType.Shipping, applicationUserId);
            var currentBillingAddress = _addressService.GetCustomerAddress(AddressType.Billing, applicationUserId);

            var addressUpdateViewModel = new AddressUpdateViewModel()
            {
                BillingAddress = Mapper.Map<AddressViewModel>(currentBillingAddress),
                ShippingAddress = Mapper.Map<AddressViewModel>(currentShippingAddress),
            };

            return View(addressUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(AddressUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var applicationUserId = User.Identity.GetUserId();
            var shippingAddress = Mapper.Map<Address>(model.ShippingAddress);
            shippingAddress.Type = AddressType.Shipping;
            shippingAddress.ApplicationUserId = applicationUserId;

            var billingAddress = Mapper.Map<Address>(model.BillingAddress);
            billingAddress.Type = AddressType.Billing;
            billingAddress.ApplicationUserId = applicationUserId;

            _addressService.UpdateAddress(billingAddress, shippingAddress, applicationUserId);

            return RedirectToAction("Payment");
        }

        public ActionResult Payment()
        {
            var applicationUserId = User.Identity.GetUserId();
            var currentShippingAddress = _addressService.GetCustomerAddress(AddressType.Shipping, applicationUserId);
            var currentBillingAddress = _addressService.GetCustomerAddress(AddressType.Billing, applicationUserId);

            var model = new ReviewOrderViewModel()
            {
                CartItems = _cartService.GetCartItems(),
                CartTotal = string.Format("{0:F}", _cartService.GetTotal()),
                CartCount = _cartService.GetCount(),
                BillingAddress = Mapper.Map<AddressViewModel>(currentBillingAddress),
                ShippingAddress = Mapper.Map<AddressViewModel>(currentShippingAddress)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(ReviewOrderViewModel reviewOrderViewModel)
        {
            if (reviewOrderViewModel.PromoCode != PromoCodeConstant.PromoCode)
            {
                ModelState.AddModelError("PromoCode", "Invalid Promo Code");
                return View(reviewOrderViewModel);
            }

            var orderId = _orderService.ProcessOrder(_cartService, User.Identity.GetUserId());

            if (orderId == 0)
            {
                ModelState.AddModelError("", "Order processing failed ");
                return View(reviewOrderViewModel);
            }

            return RedirectToAction("Complete", new { id = orderId });
        }

        public ActionResult Complete(int id)
        {
            var newOrder = _orderService.GetOrderbyOrderIdForUser(id, User.Identity.GetUserId());
            if (newOrder == null)
                return View("Error");

            return View(newOrder.Id);
        }
    }
}