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

            var addressUpdateViewModel = new AddressUpdateViewModel();

            if (currentBillingAddress != null)
            {
                addressUpdateViewModel.BillingAddress.Attn = currentBillingAddress.Attn;
                addressUpdateViewModel.BillingAddress.Line1 = currentBillingAddress.Line1;
                addressUpdateViewModel.BillingAddress.Line2 = currentBillingAddress.Line2;
                addressUpdateViewModel.BillingAddress.City = currentBillingAddress.City;
                addressUpdateViewModel.BillingAddress.State = currentBillingAddress.State;
                addressUpdateViewModel.BillingAddress.ZipCode = currentBillingAddress.ZipCode;
                addressUpdateViewModel.BillingAddress.Country = currentBillingAddress.Country;
            }

            if (currentShippingAddress != null)
            {
                addressUpdateViewModel.ShippingAddress.Attn = currentShippingAddress.Attn;
                addressUpdateViewModel.ShippingAddress.Line1 = currentShippingAddress.Line1;
                addressUpdateViewModel.ShippingAddress.Line2 = currentShippingAddress.Line2;
                addressUpdateViewModel.ShippingAddress.City = currentShippingAddress.City;
                addressUpdateViewModel.ShippingAddress.State = currentShippingAddress.State;
                addressUpdateViewModel.ShippingAddress.ZipCode = currentShippingAddress.ZipCode;
                addressUpdateViewModel.ShippingAddress.Country = currentShippingAddress.Country;
            }

            return View(addressUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(AddressUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var applicationUserId = User.Identity.GetUserId();

            var shippingAddress = new Address
            {
                Type = AddressType.Shipping,
                Attn = model.ShippingAddress.Attn,
                Line1 = model.ShippingAddress.Line1,
                Line2 = model.ShippingAddress.Line2,
                City = model.ShippingAddress.City,
                State = model.ShippingAddress.State,
                ZipCode = model.ShippingAddress.ZipCode,
                Country = model.ShippingAddress.Country,
                ApplicationUserId = applicationUserId,
            };

            var billingAddress = new Address
            {
                Type = AddressType.Billing,
                Attn = model.BillingAddress.Attn,
                Line1 = model.BillingAddress.Line1,
                Line2 = model.BillingAddress.Line2,
                City = model.BillingAddress.City,
                State = model.BillingAddress.State,
                ZipCode = model.BillingAddress.ZipCode,
                Country = model.BillingAddress.Country,
                ApplicationUserId = applicationUserId,
            };

            _addressService.UpdateAddress(billingAddress, shippingAddress, applicationUserId);
            return RedirectToAction("Payment");
        }

        public ActionResult Payment()
        {
            var applicationUserId = User.Identity.GetUserId();
            var currentShippingAddress = _addressService.GetCustomerAddress(AddressType.Shipping, applicationUserId);
            var currentBillingAddress = _addressService.GetCustomerAddress(AddressType.Billing, applicationUserId);


            var model = new ReviewOrderViewModel();

            if (currentBillingAddress != null)
            {
                model.BillingAddress.Attn = currentBillingAddress.Attn;
                model.BillingAddress.Line1 = currentBillingAddress.Line1;
                model.BillingAddress.Line2 = currentBillingAddress.Line2;
                model.BillingAddress.City = currentBillingAddress.City;
                model.BillingAddress.State = currentBillingAddress.State;
                model.BillingAddress.ZipCode = currentBillingAddress.ZipCode;
                model.BillingAddress.Country = currentBillingAddress.Country;
            }

            if (currentShippingAddress != null)
            {
                model.ShippingAddress.Attn = currentShippingAddress.Attn;
                model.ShippingAddress.Line1 = currentShippingAddress.Line1;
                model.ShippingAddress.Line2 = currentShippingAddress.Line2;
                model.ShippingAddress.City = currentShippingAddress.City;
                model.ShippingAddress.State = currentShippingAddress.State;
                model.ShippingAddress.ZipCode = currentShippingAddress.ZipCode;
                model.ShippingAddress.Country = currentShippingAddress.Country;
            }

            model.CartItems = _cartService.GetCartItems();
            model.CartTotal = string.Format("{0:F}", _cartService.GetTotal());
            model.CartCount = _cartService.GetCount();

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