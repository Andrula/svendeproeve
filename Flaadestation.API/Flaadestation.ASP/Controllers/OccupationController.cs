using Flaadestation.Service.DTO.OccupationDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccupationController : ControllerBase
    {
        private readonly IOccupationService _occupationService;

        public OccupationController(IOccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllOccupations()
        {
            var occupations = await _occupationService.GetAllOccupationsAsync();
            return Ok(occupations);
        }

        [HttpGet("GetAllOccupations")]
        public async Task<IActionResult> GetOccupation(Guid id)
        {
            var occupation = await _occupationService.GetOccupationByIdAsync(id);
            if (occupation == null)
                return NotFound();
            return Ok(occupation);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOccupation([FromBody] CreateOccupationRequestDTO request)
        {
            var occupation = new Occupation
            {
                OccupationId = Guid.NewGuid(),
                Name = request.Name
            };

            try
            {
                var created = await _occupationService.CreateOccupationAsync(occupation);
                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-if-occupation-exists")]
        public async Task<IActionResult> CheckOccupationNameAvailability([FromQuery] string name)
        {
            var available = await _occupationService.IsOccupationNameAvailableAsync(name);
            return Ok(new { available });
        }
    }
}

