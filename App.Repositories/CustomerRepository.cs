

using App.Entities.Entities;
using App.Entities.Helper;
using App.Repositories.Base;
using App.Repositories.Context;
using App.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(StoreContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Customer>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(c => c.Name.Contains(name))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(c => c.Email == email);
        }

        // Custom paged method with specific filters
        public async Task<PagedList<Customer>> GetPagedCustomersAsync(CustomerParameters parameters)
        {
            IQueryable<Customer> query = _dbSet;

            // Apply search
            query = ApplySearch(query, parameters.Search);

            // Apply email filter
            if (!string.IsNullOrWhiteSpace(parameters.Email))
            {
                query = query.Where(c => c.Email.Contains(parameters.Email.ToLower()));
            }

            // Apply phone filter
            if (!string.IsNullOrWhiteSpace(parameters.Phone))
            {
                query = query.Where(c => c.Phone.Contains(parameters.Phone));
            }

            // Apply ordering
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                query = ApplyOrdering(query, parameters.OrderBy);
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            return await Task.FromResult(
                PagedList<Customer>.ToPagedList(query, parameters.PageNumber, parameters.PageSize)
            );
        }

        // Override search logic for Customer
        protected override IQueryable<Customer> ApplySearch(IQueryable<Customer> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var lowerCaseSearchTerm = searchTerm.ToLower();

            return query.Where(c =>
                c.Name.ToLower().Contains(lowerCaseSearchTerm) ||
                c.Email.ToLower().Contains(lowerCaseSearchTerm) ||
                c.Phone.Contains(lowerCaseSearchTerm) ||
                c.Address.ToLower().Contains(lowerCaseSearchTerm)
            );
        }
    }
}
