using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.OrderDetails).ToList();
        }

        public Order? GetOrderById(int orderId)
        {
            return _context.Orders.Include(o => o.OrderDetails)
                                  .FirstOrDefault(o => o.OrderId == orderId);
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
        }
    }
}
