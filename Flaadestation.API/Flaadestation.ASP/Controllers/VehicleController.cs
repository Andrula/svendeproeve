using Flaadestation.ASP.Utils;
using Flaadestation.Service.DTO.VehicleDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetVehiclesByCompany()
        {
            try
            {

                Guid? companyIdFromClaims = AuthenticationUtils.GetCompanyIdFromClaims(User);

                if (companyIdFromClaims is Guid companyId)
                {
                    var vehicles = await _vehicleService.GetVehiclesByCompanyAsync(companyId);
                    return Ok(vehicles);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(Guid id)
        {
            var vehicleResponse = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicleResponse == null)
                return NotFound();

            return Ok(vehicleResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleRequestDTO vehicleRequest)
        {
            try
            {
                var created = await _vehicleService.CreateVehicleAsync(vehicleRequest);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleRequestDTO vehicleRequest)
        {
            try
            {
                var updated = await _vehicleService.UpdateVehicleAsync(id, vehicleRequest);
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
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            try
            {
                var deleted = await _vehicleService.DeleteVehicleAsync(id);
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
