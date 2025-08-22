using Flaadestation.Service.DTO.JobDTO;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetJobsByCompany(Guid companyId)
        {
            var jobs = await _jobService.GetJobsByCompanyAsync(companyId);
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(Guid id)
        {
            var jobResponse = await _jobService.GetJobByIdAsync(id);
            if (jobResponse == null)
                return NotFound();

            return Ok(jobResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobRequestDTO jobRequest)
        {
            try
            {
                var created = await _jobService.CreateJobAsync(jobRequest);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] JobRequestDTO jobRequest)
        {
            try
            {
                var updated = await _jobService.UpdateJobAsync(id, jobRequest);
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
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            try
            {
                var deleted = await _jobService.DeleteJobAsync(id);
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
        public async Task<IActionResult> CheckJobNameAvailability([FromQuery] Guid companyId, string name)
        {
            var decodedName = Uri.UnescapeDataString(name);
            var available = await _jobService.IsJobNameAvailableAsync(decodedName, companyId);
            return Ok(new { available });
        }
    }
}
