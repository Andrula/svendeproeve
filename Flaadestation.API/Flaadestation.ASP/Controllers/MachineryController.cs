using Flaadestation.Service.DTO.MachineryDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineryController : ControllerBase
    {
        private readonly IMachineryService _machineryService;

        public MachineryController(IMachineryService machineryService)
        {
            _machineryService = machineryService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetMachineryByCompany(Guid companyId)
        {
            var machinery = await _machineryService.GetMachineryByCompanyIdAsync(companyId);
            return Ok(machinery);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachinery(Guid id)
        {
            var machinery = await _machineryService.GetMachineryByIdAsync(id);
            if (machinery == null)
                return NotFound();
            return Ok(machinery);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableMachinery([FromQuery] Guid companyId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var machinery = await _machineryService.GetAvailableMachineryAsync(companyId, startDate, endDate);
            return Ok(machinery);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMachinery([FromBody] MachineryRequestDTO request)
        {
            try
            {
                var created = await _machineryService.CreateMachineryAsync(request);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachinery(Guid id, [FromBody] MachineryRequestDTO request)
        {
            try
            {
                var updated = await _machineryService.UpdateMachineryAsync(id, request);
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
        public async Task<IActionResult> DeleteMachinery(Guid id)
        {
            try
            {
                var deleted = await _machineryService.DeleteMachineryAsync(id);
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
