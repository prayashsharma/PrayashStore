using Microsoft.AspNet.Identity;
using PrayashStore.Models;
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
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult History(OrderHistoryForOneUserViewModel model)
        {
            ViewBag.IdSortParm = String.IsNullOrEmpty(model.SortOrder) ? "id_desc" : "";
            ViewBag.DateSortParm = model.SortOrder == "date_asc" ? "date_desc" : "date_asc";

            ViewBag.SearchedOrderId = model.OrderId;

            var userId = User.Identity.GetUserId();

            IEnumerable<Order> orders = new List<Order>();
            orders = _context.Orders.Where(x => x.ApplicationUserId == userId).ToList();

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

            model.UsersDropDownList = _context.Users.Select(s => new SelectListItem
            {
                Value = s.UserName,
                Text = s.UserName
            });

            IEnumerable<Order> orders = new List<Order>();

            if (string.IsNullOrWhiteSpace(model.SelectedUserName) && model.OrderId == null)
            {
                orders = _context.Orders.ToList();
            }
            else if (string.IsNullOrWhiteSpace(model.SelectedUserName))
            {
                orders = _context.Orders.Where(o => o.Id == model.OrderId).ToList();
            }
            else if (model.OrderId == null)
            {
                orders = _context.Orders.Where(o => o.ApplicationUser.UserName == model.SelectedUserName).ToList();
            }
            else
            {
                orders = _context.Orders
                    .Where(o => o.ApplicationUser.UserName == model.SelectedUserName && o.Id == model.OrderId)
                    .ToList();
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
            var model = _context.OrderDetails.Where(x => x.OrderId == id).ToList();
            return View(model);

        }
    }
}