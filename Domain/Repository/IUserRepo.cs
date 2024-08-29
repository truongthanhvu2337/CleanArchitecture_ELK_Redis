using Domain.Entities;
using Domain.Repository.Generic;

namespace Domain.Repository
{
    public interface IUserRepo : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Customer? GetCustomerById(int id);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        Task<Customer> GetByEmail(string Name);

    }
}
