using Domain.Entities;
using Domain.Repository;
using Infrastructure.Caching;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IRedisCaching _caching;
        public CustomerRepository(ApplicationDbContext context, IRedisCaching redisCaching) : base(context)
        {
            _context = context;
            _caching = redisCaching;
        }

        // Get all customers
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var cacheKey = $"GetAllUser";
            var cachedUsers = await _caching.GetAsync<IEnumerable<Customer>>(cacheKey);

            if (cachedUsers != null)
            {
                return cachedUsers;
            }

            var entities = await _context.Customers.Include(c => c.Orders).ToListAsync(); ;
            await _caching.SetAsync(cacheKey, entities);
            return entities;

        }

        // Get customer by Id
        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.Include(c => c.Orders)
                                     .FirstOrDefault(c => c.Id == id);
        }

        // Add a new customer
        public void AddCustomer(Customer customer)
        {
            _caching.RemoveAsync("GetAllUser");
            _context.Customers.Add(customer);
        }

        // Update an existing customer
        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        // Delete a customer
        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public async Task<Customer> GetByEmail(string Name)
        {
            return await _context.Set<Customer>().FirstOrDefaultAsync(customer => customer.Name == Name);
        }

    }
}
