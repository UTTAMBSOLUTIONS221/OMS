using DBL.Data;
using DBL.Models;
using Microsoft.EntityFrameworkCore;

namespace DBL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Creates a new customer and saves it to the database
        public async Task<Customers> Create(Customers customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        // Retrieves a customer by their ID, using AsNoTracking for read-only operations
        // This improves performance by not tracking the entity in the context, which is useful for read-only scenarios.
        public async Task<Customers?> GetCustomerById(int id)
        {
            return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
