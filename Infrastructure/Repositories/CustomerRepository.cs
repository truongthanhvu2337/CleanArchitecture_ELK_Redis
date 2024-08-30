using Castle.Core.Resource;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Caching;
using Infrastructure.Elasticsearch;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IRedisCaching _caching;
        private readonly IElasticService _elasticService;

        public CustomerRepository(ApplicationDbContext context, IRedisCaching caching, IElasticService elasticService) : base(context)
        {
            _context = context;
            _caching = caching;
            _elasticService = elasticService;
        }


        //client -> redis -> elastic search  -> db

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
        public void AddCustomer(Customer customer)
        {
            _elasticService.IndexDocumentAsync(customer);
            _caching.RemoveAsync("GetAllUser");
            _context.Customers.Add(customer);
        }

        // Update an existing customer
        public void UpdateCustomer(Customer customer)
        {
            _caching.RemoveAsync("GetAllUser");
            _context.Customers.Update(customer);
        }

        // Delete a customer
        //public async Task<bool> DeleteCustomer(int id)
        //{
        //    await _caching.RemoveAsync("GetAllUser");
        //    var customer = _context.Customers.Find(id);
        //    if (customer != null)
        //    {
        //        _context.Customers.Remove(customer);
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<Customer> GetByEmail(string Name)
        {
            return await _context.Set<Customer>().FirstOrDefaultAsync(customer => customer.Name == Name);
        }

    }
}
