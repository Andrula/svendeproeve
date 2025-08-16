using Azure.Core;
using Flaadestation.ASP.DTO;
using Flaadestation.ASP.DTO.CompanyDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequestDTO request)
        {
            var company = new Company
            {
                CompanyId = Guid.NewGuid(),
                Name = request.Name,
                AddressId = request.AddressId
            };

            try
            {
                var created = await _companyService.CreateCompanyAsync(company);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-name/{name}")]
        public async Task<IActionResult> CheckCompanyNameAvailability(string name)
        {
            var decodedName = Uri.UnescapeDataString(name);
            var available = await _companyService.IsCompanyNameAvailableAsync(decodedName);
            return Ok(new { available });
        }
    }
}
