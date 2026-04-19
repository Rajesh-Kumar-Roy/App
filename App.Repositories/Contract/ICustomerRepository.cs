
using App.Entities.Entities;
using App.Entities.Helper;

namespace App.Repositories.Contract
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<List<Customer>> SearchByNameAsync(string name);
        Task<bool> EmailExistsAsync(string email);
        Task<PagedList<Customer>> GetPagedCustomersAsync(CustomerParameters parameters);
    }
}
