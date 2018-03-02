using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.Services.Interfaces
{
    public interface IOrderService
    {
        int ProcessOrder(ICartService cartService, string userId);
        Order GetOrderbyOrderIdForUser(int orderId, string userId);
        Order GetOrderByOrderId(int orderId);
        IEnumerable<Order> GetAllOrdersForUser(string userId);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<OrderDetail> GetOrderDetails(int orderId);
    }
}
