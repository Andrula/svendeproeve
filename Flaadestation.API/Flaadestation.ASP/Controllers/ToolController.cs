using Flaadestation.ASP.Utils;
using Flaadestation.Service.DTO.ToolDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : Controller
    {
        private readonly IToolService _toolService;
        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [HttpGet("company")]
        public async Task<IActionResult> GetToolsByCompany()
        {
            try
            {
                Guid? companyIdFromClaims = AuthenticationUtils.GetCompanyIdFromClaims(User);

                if (companyIdFromClaims is Guid companyId)
                {
                    var tools = await _toolService.GetToolsByCompanyAsync(companyId);
                    return Ok(tools);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTool(Guid id)
        {
            var toolResponse = await _toolService.GetToolByIdAsync(id);
            if (toolResponse == null)
                return NotFound();

            return Ok(toolResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTool([FromBody] ToolRequestDTO toolRequest)
        {
            try
            {
                var created = await _toolService.CreateToolAsync(toolRequest);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTool(Guid id, [FromBody] ToolRequestDTO toolRequest)
        {
            try
            {
                var updated = await _toolService.UpdateToolAsync(id, toolRequest);
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
        public async Task<IActionResult> DeleteTool(Guid id)
        {
            try
            {
                var deleted = await _toolService.DeleteToolAsync(id);
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
