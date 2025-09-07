using Flaadestation.ASP.Utils;
using Flaadestation.Service.DTO.EmployeeDTO;
using Flaadestation.Service.DTO.LicenseDTO;
using Flaadestation.Service.Interfaces;
using Flaadestation.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : Controller
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetLicensesByCompany()
        {
            try
            {

                Guid? companyIdFromClaims = AuthenticationUtils.GetCompanyIdFromClaims(User);

                if (companyIdFromClaims is Guid companyId)
                {
                    var licenses = await _licenseService.GetLicensesByCompanyIdAsync(companyId);
                    return Ok(licenses);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLicenseByUser(string userId)
        {
            var license = await _licenseService.GetLicensesByUserIdAsync(userId);

            if (license == null)
                return NotFound();

            return Ok(license);
        }

        [HttpGet("key/{key}")]
        public async Task<IActionResult> GetLicenseByLicenseKey(Guid key)
        {
            var license = await _licenseService.GetLicenseByLicenseKeyAsync(key);

            if (license == null)
                return NotFound();

            return Ok(license);
        }


        [HttpPost]
        public async Task<IActionResult> CreateLicense([FromBody] LicenseRequestDTO licenseRequest)
        {
            try
            {
                var created = await _licenseService.CreateLicenseAsync(licenseRequest);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(Guid licenseId, [FromBody] LicenseRequestDTO licenseRequest)
        {
            try
            {
                var updated = await _licenseService.UpdateLicenseByIdAsync(licenseId, licenseRequest);
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
        public async Task<IActionResult> DeleteLicense(Guid id)
        {
            try
            {
                var deleted = await _licenseService.DeleteLicenseAsync(id);
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
