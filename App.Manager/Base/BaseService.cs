using App.Entities.Entities.Base;
using App.Entities.Helper;
using App.Managers.Contract;
using App.Managers.EntityDtos;
using App.Repositories.Contract;
using AutoMapper;

namespace App.Managers.Base
{
    public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>
        : IBaseService<TDto, TCreateDto, TUpdateDto>
        where TEntity : class, IBaseEntity, IAudit
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(created);
        }

        public virtual async Task UpdateAsync(TUpdateDto updateDto)
        {
            // Get Id from updateDto
            var idProperty = typeof(TUpdateDto).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("UpdateDto must have an Id property");
            }

            var id = (long)idProperty.GetValue(updateDto)!;
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} with id {id} not found");
            }

            // Map updateDto to existing entity
            _mapper.Map(updateDto, entity);
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(TEntity).Name} with id {id} not found");
            }

            await _repository.DeleteAsync(id);
        }

        public virtual async Task<PagedResultDto<TDto>> GetPagedAsync(RequestParameters parameters)
        {
            var pagedEntities = await _repository.GetPagedAsync(parameters);
            return _mapper.Map<PagedResultDto<TDto>>(pagedEntities);
        }

        public virtual async Task<bool> ExistsAsync(long id)
        {
            return await _repository.ExistsAsync(id);
        }
    }
}
