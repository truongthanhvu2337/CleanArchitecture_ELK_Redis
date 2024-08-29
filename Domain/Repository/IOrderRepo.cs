using Domain.Entities;

namespace Domain.Repository
{
    public interface IOrderRepo
    {
        IEnumerable<Order> GetAllOrders();
        Order? GetOrderById(int id);
        void AddOrder(Order Order);
        void UpdateOrder(Order Order);
        void DeleteOrder(int id);
    }
}
