using App.Entities.Entities;
using App.Entities.Helper;
using App.Managers.Base;
using App.Managers.Contract;
using App.Managers.EntityDtos;
using App.Repositories.Contract;
using AutoMapper;


namespace App.Managers
{
    public class CustomerService : BaseService<Customer, CustomerDto, CreateCustomerDto, UpdateCustomerDto>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper)
            : base(customerRepository, mapper)
        {
            _customerRepository = customerRepository;
        }

        #region Override Create and Update with Business Logic

        public override async Task<CustomerDto> CreateAsync(CreateCustomerDto createDto)
        {
            // Business validation: Check if email already exists
            var existingCustomer = await _customerRepository.GetByEmailAsync(createDto.Email);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException($"Customer with email '{createDto.Email}' already exists");
            }

            // Call base implementation
            return await base.CreateAsync(createDto);
        }

        public override async Task UpdateAsync(UpdateCustomerDto updateDto)
        {
            var customer = await _customerRepository.GetByIdAsync(updateDto.Id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with id {updateDto.Id} not found");
            }

            // Business validation: Check if email is being changed to an existing email
            if (customer.Email != updateDto.Email)
            {
                var existingCustomer = await _customerRepository.GetByEmailAsync(updateDto.Email);
                if (existingCustomer != null && existingCustomer.Id != updateDto.Id)
                {
                    throw new InvalidOperationException($"Customer with email '{updateDto.Email}' already exists");
                }
            }

            // Call base implementation
            await base.UpdateAsync(updateDto);
        }

        #endregion

        #region Custom Methods

        public async Task<CustomerDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty", nameof(email));
            }

            var customer = await _customerRepository.GetByEmailAsync(email);
            return customer == null ? null : _mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }

            var customers = await _customerRepository.SearchByNameAsync(name);
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<PagedResultDto<CustomerDto>> GetPagedCustomersAsync(CustomerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var pagedCustomers = await _customerRepository.GetPagedCustomersAsync(parameters);
            return _mapper.Map<PagedResultDto<CustomerDto>>(pagedCustomers);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return await _customerRepository.EmailExistsAsync(email);
        }

        #endregion
    }
}
