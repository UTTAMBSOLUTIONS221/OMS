using DBL.Models;

namespace DBL.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customers> Create(Customers customer);
        Task<Customers> GetCustomerById(int id);
    }
}
