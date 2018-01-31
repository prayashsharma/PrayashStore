using Microsoft.AspNet.Identity;
using PrayashStore.Constants;
using PrayashStore.Helpers;
using PrayashStore.Models;
using PrayashStore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckOutController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Address()
        {
            var applicationUserId = User.Identity.GetUserId();
            var currentShippingAddress = _context.Addresses
                .SingleOrDefault(x => x.Type == AddressType.Shipping && x.ApplicationUserId == applicationUserId);
            var currentBillingAddress = _context.Addresses
                 .SingleOrDefault(x => x.Type == AddressType.Billing && x.ApplicationUserId == applicationUserId);

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

            // Get the current shipping address for this user
            var currentShippingAddress = _context.Addresses
                .SingleOrDefault(x => x.Type == AddressType.Shipping && x.ApplicationUserId == applicationUserId);

            if (currentShippingAddress == null)
            {
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
                _context.Addresses.Add(shippingAddress);
            }
            else
            {
                currentShippingAddress.Attn = model.ShippingAddress.Attn;
                currentShippingAddress.Line1 = model.ShippingAddress.Line1;
                currentShippingAddress.Line2 = model.ShippingAddress.Line2;
                currentShippingAddress.City = model.ShippingAddress.City;
                currentShippingAddress.State = model.ShippingAddress.State;
                currentShippingAddress.ZipCode = model.ShippingAddress.ZipCode;
                currentShippingAddress.Country = model.ShippingAddress.Country;
            }

            // Get the current billing address for this user
            var currentBillingAddress = _context.Addresses
                .SingleOrDefault(x => x.Type == AddressType.Billing && x.ApplicationUserId == applicationUserId);

            if (currentBillingAddress == null)
            {
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
                _context.Addresses.Add(billingAddress);
            }
            else
            {
                currentBillingAddress.Attn = model.BillingAddress.Attn;
                currentBillingAddress.Line1 = model.BillingAddress.Line1;
                currentBillingAddress.Line2 = model.BillingAddress.Line2;
                currentBillingAddress.City = model.BillingAddress.City;
                currentBillingAddress.State = model.BillingAddress.State;
                currentBillingAddress.ZipCode = model.BillingAddress.ZipCode;
                currentBillingAddress.Country = model.BillingAddress.Country;
            }

            _context.SaveChanges();

            return RedirectToAction("Payment");
        }

        public ActionResult Payment()
        {
            var applicationUserId = User.Identity.GetUserId();
            var currentShippingAddress = _context.Addresses
                .SingleOrDefault(x => x.Type == AddressType.Shipping && x.ApplicationUserId == applicationUserId);
            var currentBillingAddress = _context.Addresses
                 .SingleOrDefault(x => x.Type == AddressType.Billing && x.ApplicationUserId == applicationUserId);
            var cart = ShoppingCartHelper.GetCart(_context, HttpContext);
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

            model.CartItems = cart.GetCartItems();
            model.CartTotal = string.Format("{0:F}", cart.GetTotal());
            model.CartCount = cart.GetCount();

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

            var order = new Order();
            //TryUpdateModel(order);

            try
            {
                //Get Shopping cart
                var cart = ShoppingCartHelper.GetCart(_context, this.HttpContext);

                //create order
                order.ApplicationUserId = User.Identity.GetUserId();
                order.Total = cart.GetTotal();
                order.DateCreated = DateTime.Now;

                //Save Order
                _context.Orders.Add(order);
                _context.SaveChanges();

                //create Order details                
                CreateOrderDetails(order, cart);

                // Empty the shopping cart
                cart.EmptyCart();

                return RedirectToAction("Complete",
                    new { id = order.Id });
            }
            catch (Exception e)
            {
                //Invalid - redisplay with errors
                ModelState.AddModelError("", "Order processing failed " + e.Message);
                return View(order);
            }

        }

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order

            var userId = User.Identity.GetUserId();
            bool isValid = _context.Orders.Any(
                o => o.Id == id &&
                o.ApplicationUserId == userId);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        private void CreateOrderDetails(Order order, ShoppingCartHelper cart)
        {
            var cartItems = cart.GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };

                _context.OrderDetails.Add(orderDetail);

            }

            // Save the order
            _context.SaveChanges();

        }

    }
}