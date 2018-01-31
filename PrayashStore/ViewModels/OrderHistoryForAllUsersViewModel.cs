using PrayashStore.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PrayashStore.ViewModels
{
    public class OrderHistoryForAllUsersViewModel
    {
        public int? OrderId { get; set; }
        public string SortOrder { get; set; }
        public OrderHistoryForAllUsersViewModel()
        {
            Orders = new List<Order>();
            UsersDropDownList = new List<SelectListItem>();
        }
        public string SelectedUserName { get; set; }
        public IEnumerable<SelectListItem> UsersDropDownList { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}