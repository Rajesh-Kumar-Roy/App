

using App.Managers.EntityDtos;
using App.Managers.Contract;
using App.Entities.Entities;
using App.Entities.Helper;

namespace App.Managers.Contract
{
    public interface ICustomerService : IBaseService<CustomerDto, CreateCustomerDto, UpdateCustomerDto>
    {
        Task<CustomerDto?> GetByEmailAsync(string email);
        Task<List<CustomerDto>> SearchByNameAsync(string name);
        Task<PagedResultDto<CustomerDto>> GetPagedCustomersAsync(CustomerParameters parameters);
        Task<bool> EmailExistsAsync(string email);
    }

}
