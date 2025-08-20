using Flaadestation.ASP.DTO.CustomerDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCustomersByCompany(Guid companyId)
        {
            var customers = await _customerService.GetCustomersByCompanyAsync(companyId);

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            var response = new CustomerResponseDTO
            {
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                CompanyId = customer.CompanyId,
                AddressId = customer.AddressId,
            };

            return Ok(response);
        }

        [HttpGet("{id}/with-jobs")]
        public async Task<IActionResult> GetCustomerWithJobs(Guid id)
        {
            var customer = await _customerService.GetCustomerWithJobsAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequestDTO request)
        {
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CompanyId = request.CompanyId,
                AddressId = request.AddressId
            };

            try
            {
                var created = await _customerService.CreateCustomerAsync(customer);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequestDTO request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                AddressId = request.AddressId
            };

            try
            {
                var updated = await _customerService.UpdateCustomerAsync(id, customer);
                if (updated == null)
                    return NotFound();

                var updatedRepsonse = new CustomerResponseDTO
                {
                    Name = updated.Name,
                    Email = updated.Email,
                    Phone = updated.Phone,
                    CompanyId = updated.CompanyId,
                    AddressId = updated.AddressId
                };
                return Ok(updatedRepsonse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var deleted = await _customerService.DeleteCustomerAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckCustomerEmailAvailability([FromQuery] string email, [FromQuery] Guid companyId)
        {
            var available = await _customerService.IsCustomerEmailAvailableAsync(email, companyId);
            return Ok(new { available });
        }
    }
}
