using Azure.Core;
using Flaadestation.Service.DTO;
using Flaadestation.Service.DTO.CompanyDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Flaadestation.ASP.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRequestDTO request)
        {
            try
            {
                var created = await _companyService.CreateCompanyAsync(request);
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
