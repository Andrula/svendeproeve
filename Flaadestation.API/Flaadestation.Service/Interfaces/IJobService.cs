using Flaadestation.Repository.Database.Entities;
using Flaadestation.Service.DTO.JobDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobResponseDTO>> GetJobsByCompanyAsync(Guid companyId);
        Task<JobResponseDTO?> GetJobByIdAsync(Guid jobId);
        Task<JobResponseDTO> CreateJobAsync(JobRequestDTO jobRequest);
        Task<JobResponseDTO?> UpdateJobAsync(Guid jobId, JobRequestDTO jobRequest);
        Task<bool> DeleteJobAsync(Guid jobId);
        Task<bool> IsJobNameAvailableAsync(string name, Guid companyId);
    }
}
