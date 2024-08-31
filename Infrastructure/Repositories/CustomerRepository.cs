using Castle.Core.Resource;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Caching;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Elasticsearch;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IRedisCaching _caching;
        private readonly IElasticService<Customer> _elasticService;

        public CustomerRepository(ApplicationDbContext context, IRedisCaching caching, IElasticService<Customer> elasticService) : base(context)
        {
            _context = context;
            _caching = caching;
            _elasticService = elasticService;
        }



        // Get all customers
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            //var cacheKey = $"GetAllUser";
            //var cachedUsers = await _caching.GetAsync<IEnumerable<Customer>>(cacheKey);

            //if (cachedUsers != null)
            //{
            //    return cachedUsers;
            //}
            //var a = await _elasticService.GetAll();
            //var entities = await _context.Customers.Include(c => c.Orders).ToListAsync(); ;
            //await _caching.SetAsync(cacheKey, entities);
            var entities = await _elasticService.GetAll();
            return entities;

        }

        // Get customer by Id
        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.Include(c => c.Orders)
                                     .FirstOrDefault(c => c.Id == id);
        }

        // Add a new customer
        public async Task AddCustomer(Customer customer)
        {
                
            var q = await _context.Customers.MaxAsync(x => x.Id);
            await _elasticService.IndexDocumentWithKeywordAsync(new Customer
            {
                Id = q + 1,
                Name = customer.Name,
                Address = customer.Address,
            }, q + 1);
            await _caching.RemoveAsync("GetAllUser");

            await _context.Customers.AddAsync(customer);
        }

        // Update an existing customer
        public void UpdateCustomer(Customer customer)
        {
            _caching.RemoveAsync("GetAllUser");
            _context.Customers.Update(customer);
        }

        // Delete a customer
        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return true;
            }
            await _caching.RemoveAsync("GetAllUser");
            return false;
        }

        public async Task<Customer> GetByEmail(string Name)
        {
            return await _context.Set<Customer>().FirstOrDefaultAsync(customer => customer.Name == Name);
        }

    }
}
