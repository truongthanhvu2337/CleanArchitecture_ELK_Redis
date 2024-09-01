using Domain.Entities;
using Domain.Repository.Generic;

namespace Domain.Repository
{
    public interface IUserRepo : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer?> GetCustomerById(int id);
        Task AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int id);
        Task<Customer> GetByEmail(string Name);
        Task<IEnumerable<Customer>> Pagination(int page, int pageSize);

    }
}
