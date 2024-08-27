using Domain.Entities;

namespace Application.Repository
{
    public interface IUserRepo
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer? GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);

    }
}
