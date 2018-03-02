using Microsoft.AspNet.Identity;
using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context, IOrderService orderService, ApplicationUserManager userManager)
        {
            _context = context;
            _orderService = orderService;
            _userManager = userManager;
        }
        public ActionResult History(OrderHistoryForOneUserViewModel model)
        {
            ViewBag.IdSortParm = String.IsNullOrEmpty(model.SortOrder) ? "id_desc" : "";
            ViewBag.DateSortParm = model.SortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.SearchedOrderId = model.OrderId;

            var orders = _orderService.GetAllOrdersForUser(User.Identity.GetUserId());

            if (model.OrderId != null)
            {
                orders = orders.Where(x => x.Id == model.OrderId).ToList();
            }

            switch (model.SortOrder)
            {
                case "id_desc":
                    orders = orders.OrderByDescending(x => x.Id).ToList();
                    break;
                case "date_asc":
                    orders = orders.OrderBy(x => x.DateCreated).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(x => x.DateCreated).ToList();
                    break;
                default:
                    orders = orders.OrderBy(x => x.Id).ToList();
                    break;
            }
            model.Orders = orders;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult HistoryForAllUsers(OrderHistoryForAllUsersViewModel model)
        {
            ViewBag.IdSortParm = String.IsNullOrEmpty(model.SortOrder) ? "id_desc" : "";
            ViewBag.DateSortParm = model.SortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.UserSortParm = model.SortOrder == "user_asc" ? "user_desc" : "user_asc";

            ViewBag.SelectedUserName = model.SelectedUserName;
            ViewBag.SearchedOrderId = model.OrderId;

            model.UsersDropDownList = _userManager.Users.Select(s => new SelectListItem
            {
                Value = s.UserName,
                Text = s.UserName
            });

            var orders = new List<Order>();
            string userId = null;

            if (model.SelectedUserName != null)
            {
                userId = _userManager.FindByName(model.SelectedUserName).Id;
            }


            if (string.IsNullOrWhiteSpace(model.SelectedUserName) && model.OrderId == null)
            {
                orders = _orderService.GetAllOrders().ToList();
            }
            else if (string.IsNullOrWhiteSpace(userId))
            {
                var order = _orderService.GetOrderByOrderId(model.OrderId.GetValueOrDefault());
                if (order != null)
                    orders.Add(order);
            }
            else if (model.OrderId == null)
            {
                orders = _orderService.GetAllOrdersForUser(userId).ToList();
            }
            else
            {
                var order = _orderService.GetOrderbyOrderIdForUser(model.OrderId.GetValueOrDefault(), userId);
                if (order != null)
                    orders.Add(order);
            }

            switch (model.SortOrder)
            {
                case "id_desc":
                    orders = orders.OrderByDescending(x => x.Id).ToList();
                    break;
                case "date_asc":
                    orders = orders.OrderBy(x => x.DateCreated).ToList();
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(x => x.DateCreated).ToList();
                    break;
                case "user_asc":
                    orders = orders.OrderBy(x => x.ApplicationUser.UserName).ToList();
                    break;
                case "user_desc":
                    orders = orders.OrderByDescending(x => x.ApplicationUser.UserName).ToList();
                    break;
                default:
                    orders = orders.OrderBy(x => x.Id).ToList();
                    break;
            }

            model.Orders = orders;
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var model = _orderService.GetOrderDetails(id);
            return View(model);

        }
    }
}