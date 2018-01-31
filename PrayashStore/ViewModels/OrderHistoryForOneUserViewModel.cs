using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.ViewModels
{
    public class OrderHistoryForOneUserViewModel
    {
        public OrderHistoryForOneUserViewModel()
        {
            Orders = new List<Order>();
        }
        public int? OrderId { get; set; }
        public string SortOrder { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}