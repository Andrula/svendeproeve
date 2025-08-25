using Flaadestation.Service.DTO.BaseDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Flaadestation.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IBaseService _baseService;

        public BaseController(IBaseService baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetBasesByCompany(Guid companyId)
        {
            var bases = await _baseService.GetBasesByCompanyAsync(companyId);
            return Ok(bases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBase(Guid id)
        {
            var baseEntity = await _baseService.GetBaseByIdAsync(id);
            if (baseEntity == null)
                return NotFound();

            return Ok(baseEntity);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBase([FromBody] BaseRequestDTO request)
        {
            try
            {
                var response = await _baseService.CreateBaseAsync(request);

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBase(Guid id, [FromBody] BaseRequestDTO request)
        {
            try
            {
                var updated = await _baseService.UpdateBaseAsync(id, request);
                if (updated == null)
                    return NotFound();

                var updatedBaseResponse = new BaseRequestDTO
                {
                    BaseId = updated.BaseId,
                    Name = updated.Name,
                    CompanyId = updated.CompanyId,
                    AddressId = updated.AddressId
                };

                return Ok(updatedBaseResponse);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBase(Guid id)
        {
            try
            {
                var deleted = await _baseService.DeleteBaseAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-name/{name}")]
        public async Task<IActionResult> CheckBaseNameAvailability([FromQuery] Guid companyId, string name)
        {
            var decodedName = Uri.UnescapeDataString(name);
            var available = await _baseService.IsBaseNameAvailableAsync(decodedName, companyId);
            return Ok(new { available });
        }
    }
}
