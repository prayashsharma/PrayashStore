using System;
using System.Collections.Generic;

namespace PrayashStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Total { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}