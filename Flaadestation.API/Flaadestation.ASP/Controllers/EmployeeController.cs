using Flaadestation.Service.DTO.EmployeeDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompany(Guid companyId)
        {
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employeeResponse = await _employeeService.GetEmployeeByIdAsync(id);
            if (employeeResponse == null)
                return NotFound();
            return Ok(employeeResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequestDTO employeeRequest)
        {
            try
            {
                var created = await _employeeService.CreateEmployeeAsync(employeeRequest);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeRequestDTO employeeRequest)
        {
            try
            {
                var updated = await _employeeService.UpdateEmployeeAsync(id, employeeRequest);
                if (updated == null)
                    return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}