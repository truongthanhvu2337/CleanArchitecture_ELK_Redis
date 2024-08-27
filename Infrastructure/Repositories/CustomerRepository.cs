using Application.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all customers
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.Include(c => c.Orders).ToList();
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
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        // Update an existing customer
        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        // Delete a customer
        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        // Save changes (optional utility method)
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
