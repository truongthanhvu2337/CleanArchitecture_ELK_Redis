using Domain.Entities;
using Domain.Repository;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Infrastructure.Caching;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Elasticsearch;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

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
            var cacheKey = $"GetAllUser";
            var cachedUsers = await _caching.GetAsync<IEnumerable<Customer>>(cacheKey);

            if (cachedUsers != null)
            {
                return cachedUsers;
            }

            //var entities = await _context.Customers.Include(c => c.Orders).ToListAsync();

            //get data from elastic search then cache in redis
            var entities = await _elasticService.GetAll();
            await _caching.SetAsync(cacheKey, entities);
            
            return entities;

        }

        // Get customer by Id
        public async Task<Customer?> GetCustomerById(int id)
        {
            return await _context.Customers.Include(c => c.Orders)
                                     .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Add a new customer
        public async Task AddCustomer(Customer customer)
        {
            var newId = 1;
            if (_context.Customers.Any())
            {
                var maxId = _context.Customers.Max(x => (int?)x.Id) ?? 0;
                newId = maxId + 1;
            }
            await _elasticService.IndexDocumentWithKeywordAsync(new Customer
            {
                Id = newId,
                Name = customer.Name,
                Address = customer.Address,
            }, newId);

            await _caching.RemoveAsync("GetAllUser");
            await _context.Customers.AddAsync(customer);
        }

        // Update an existing customer
        public async Task UpdateCustomer(Customer customer)
        {
            await _elasticService.Update(customer, customer.Id);
            await _caching.RemoveAsync("GetAllUser");
            _context.Customers.Update(customer);
        }

        // Delete a customer
        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                await _elasticService.Remove(id);
                await _caching.RemoveAsync("GetAllUser");
                _context.Customers.Remove(customer);
                return true;
            }
            return false;
        }

        public async Task<Customer> GetByEmail(string Name)
        {
            return await _context.Set<Customer>().FirstOrDefaultAsync(customer => customer.Name == Name);
        }

        public async Task<IEnumerable<Customer>> Pagination(int page, int pageSize)
        {
            int from = (page - 1) * pageSize;
            var paginate = new SearchRequestDescriptor<Customer>()
                .From(from)
                .Size(pageSize)
                .Query(q => q.MatchAll(new MatchAllQuery()));


            return await _elasticService.FilterAsync(paginate);
        }

    }
}
