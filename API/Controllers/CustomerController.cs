using App.Entities.Helper;
using App.Managers.Contract;
using App.Managers.EntityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Get paginated customers
        /// </summary>
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<CustomerDto>>> GetPaged(
            [FromQuery] CustomerParameters parameters)
        {
            var result = await _customerService.GetPagedCustomersAsync(parameters);

            // Add pagination metadata to response headers
            var metadata = new
            {
                result.CurrentPage,
                result.PageSize,
                result.TotalCount,
                result.TotalPages,
                result.HasPrevious,
                result.HasNext
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            return Ok(result);
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(long id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound(new { message = $"Customer with id {id} not found" });
            }

            return Ok(customer);
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customer = await _customerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update existing customer
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] UpdateCustomerDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "Id mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _customerService.UpdateAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete customer (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Search customers by name
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<List<CustomerDto>>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Name parameter is required" });
            }

            var customers = await _customerService.SearchByNameAsync(name);
            return Ok(customers);
        }

        /// <summary>
        /// Get customer by email
        /// </summary>
        [HttpGet("by-email")]
        public async Task<ActionResult<CustomerDto>> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email parameter is required" });
            }

            var customer = await _customerService.GetByEmailAsync(email);

            if (customer == null)
            {
                return NotFound(new { message = $"Customer with email '{email}' not found" });
            }

            return Ok(customer);
        }

        /// <summary>
        /// Check if customer exists
        /// </summary>
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> Exists(long id)
        {
            var exists = await _customerService.ExistsAsync(id);
            return Ok(new { exists });
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email parameter is required" });
            }

            var exists = await _customerService.EmailExistsAsync(email);
            return Ok(new { exists });
        }
    }
}
