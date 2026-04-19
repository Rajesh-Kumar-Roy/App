using App.Entities.Entities.Base;
using App.Entities.Helper;
using App.Repositories.Context;
using App.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace App.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity, IAudit
    {
        protected readonly StoreContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(StoreContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDelete = true;
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(long id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        // Pagination implementation
        public virtual async Task<PagedList<T>> GetPagedAsync(RequestParameters parameters)
        {
            IQueryable<T> query = _dbSet;

            // Apply search (override in derived class for specific search logic)
            query = ApplySearch(query, parameters.Search);

            // Apply ordering
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                query = ApplyOrdering(query, parameters.OrderBy);
            }
            else
            {
                // Default ordering by Id descending
                query = query.OrderByDescending(e => e.Id);
            }

            // Return paged result
            return await Task.FromResult(
                PagedList<T>.ToPagedList(query, parameters.PageNumber, parameters.PageSize)
            );
        }

        // Virtual method for search logic (can be overridden in derived classes)
        protected virtual IQueryable<T> ApplySearch(IQueryable<T> query, string? searchTerm)
        {
            // Base implementation does nothing
            // Override in derived repository for specific search logic
            return query;
        }

        // Apply ordering using Dynamic LINQ
        protected virtual IQueryable<T> ApplyOrdering(IQueryable<T> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return query;

            try
            {
                // Support for multiple order by: "Name asc, CreatedAt desc"
                return query.OrderBy(orderBy);
            }
            catch
            {
                // If ordering fails, return original query
                return query;
            }
        }

        // Additional helper methods
        public virtual async Task HardDeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<List<T>> GetAllIncludingDeletedAsync()
        {
            return await _dbSet.IgnoreQueryFilters().ToListAsync();
        }

        public virtual async Task RestoreAsync(long id)
        {
            var entity = await _dbSet.IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity != null && entity.IsDelete)
            {
                entity.IsDelete = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
