using PrayashStore.Models;
using PrayashStore.Repositories.Interfaces;
using PrayashStore.Services.Interfaces;
using PrayashStore.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace PrayashStore.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _orderRepository = UnitOfWork.GetRepository<Order>();
            _orderDetailRepository = UnitOfWork.GetRepository<OrderDetail>();
        }

        public IRepository<Order> OrderRepository
        {
            get { return _orderRepository; }
        }

        public IRepository<OrderDetail> OrderDetailRepository
        {
            get { return _orderDetailRepository; }
        }

        public int ProcessOrder(ICartService cartService, string userId)
        {
            var order = new Order();
            try
            {
                order.ApplicationUserId = userId;
                order.Total = cartService.GetTotal();
                order.DateCreated = DateTime.Now;

                OrderRepository.Add(order);

                foreach (var item in cartService.GetCartItems())
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        UnitPrice = item.Product.Price,
                        Quantity = item.Count
                    };

                    OrderDetailRepository.Add(orderDetail);
                }

                UnitOfWork.Complete();
                cartService.EmptyCart();

                return order.Id;
            }
            catch (Exception e)
            {
                //Invalid - redisplay with errors
                return 0;
            }
        }

        public Order GetOrderbyOrderIdForUser(int orderId, string userId)
        {
            return _orderRepository.SingleOrDefault(x => x.Id == orderId && x.ApplicationUserId == userId);
        }

        public IEnumerable<Order> GetAllOrdersForUser(string userId)
        {
            return _orderRepository.Find(x => x.ApplicationUserId == userId);
        }

        public Order GetOrderByOrderId(int orderId)
        {
            return _orderRepository.Get(orderId);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            return _orderDetailRepository.Find(x => x.OrderId == orderId);
        }
    }
}