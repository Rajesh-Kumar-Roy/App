using App.Entities.Helper;
using App.Managers.EntityDtos;

namespace App.Managers.Contract
{
    public interface IBaseService<TDto, TCreateDto, TUpdateDto>
      where TDto : class
      where TCreateDto : class
      where TUpdateDto : class
    {
        Task<TDto?> GetByIdAsync(long id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto> CreateAsync(TCreateDto createDto);
        Task UpdateAsync(TUpdateDto updateDto);
        Task DeleteAsync(long id);
        Task<PagedResultDto<TDto>> GetPagedAsync(RequestParameters parameters);
        Task<bool> ExistsAsync(long id);
    }
}
