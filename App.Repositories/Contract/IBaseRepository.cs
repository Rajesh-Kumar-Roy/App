using App.Entities.Entities.Base;
using App.Entities.Helper;

namespace App.Repositories.Contract
{
    public interface IBaseRepository<T> where T : class
    {
        // Original methods
        Task<T?> GetByIdAsync(long id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);

        // Pagination methods
        Task<PagedList<T>> GetPagedAsync(RequestParameters parameters);
    }
}
