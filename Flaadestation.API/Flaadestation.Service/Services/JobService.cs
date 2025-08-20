using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Database;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flaadestation.Service.DTO.JobDTO;

namespace Flaadestation.Service.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ApplicationDBContext _context;

        public JobService(IJobRepository jobRepository, IStorageRepository storageRepositor, ApplicationDBContext context)
        {
            _jobRepository = jobRepository;
            _storageRepository = storageRepositor;
            _context = context;
        }

        // Metode til at oprette en job med et tilknyttet storage entitet.
        public async Task<JobResponseDTO> CreateJobAsync(JobRequestDTO jobRequest)
        {
            var nameExists = await _jobRepository.JobExistsByTitleAsync(jobRequest.Title, jobRequest.CompanyId);
            if (nameExists)
            {
                throw new InvalidOperationException($"Job med titlen '{jobRequest.Title}' eksisterer allerede for denne virksomhed.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var storage = new Storage();

                await _storageRepository.AddAsync(storage);

                var job = MapJobRequestToJob(jobRequest);

                job.StorageId = storage.StorageId;

                var createdJob = await _jobRepository.AddAsync(job);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapJobToJobResponse(createdJob);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// Metode til at slette job.
        public async Task<bool> DeleteJobAsync(Guid id)
        {
            var jobEntity = await _jobRepository.GetJobByIdAsync(id);
            if (jobEntity == null)
                return false;

            if (jobEntity.Storage?.StorageItems.Any() == true)
            {
                throw new InvalidOperationException("Kan ikke slette job der har ting opbevaret. Flyt alle items først.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (jobEntity.Storage != null)
                {
                    _storageRepository.Delete(jobEntity.Storage.StorageId);
                }

                _jobRepository.Delete(id);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Metode til at hente job via. ID.
        public async Task<JobResponseDTO?> GetJobByIdAsync(Guid jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);
            return job is null ? null : MapJobToJobResponse(job);
        }

        // Metode til at hente virksomhedens jobr.
        public async Task<IEnumerable<JobResponseDTO>> GetJobsByCompanyAsync(Guid companyId)
        {
            var jobs = await _jobRepository.GetJobsByCompanyAsync(companyId);
            return jobs.Select(MapJobToJobResponse);
        }

        // Metode til at om navn på job allerede eksisterer.
        public async Task<bool> IsJobNameAvailableAsync(string name, Guid companyId)
        {
            return !await _jobRepository.JobExistsByTitleAsync(name, companyId);
        }

        // Metode til at opdatere informmationer på job.
        public async Task<JobResponseDTO?> UpdateJobAsync(Guid jobId, JobRequestDTO jobRequest)
        {
            var existingJob = await _jobRepository.GetByIdAsync(jobId);
            if (existingJob == null)
                return null;

            if (existingJob.Title != jobRequest.Title)
            {
                var nameExists = await _jobRepository.JobExistsByTitleAsync(jobRequest.Title, existingJob.CompanyId);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Job med navnet '{jobRequest.Title}' eksisterer allerede for denne virksomhed.");
                }
            }

            existingJob.Title = jobRequest.Title;
            existingJob.Description = jobRequest.Description;
            existingJob.ScheduledStart = jobRequest.ScheduledStart;
            existingJob.ScheduledEnd = jobRequest.ScheduledEnd;
            existingJob.AddressId = jobRequest.AddressId;

            _jobRepository.Update(existingJob);
            await _jobRepository.SaveChangesAsync();
            return MapJobToJobResponse(existingJob);
        }

        private JobResponseDTO MapJobToJobResponse(Job job)
        {
            return new JobResponseDTO
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                ScheduledStart = job.ScheduledStart,
                ScheduledEnd = job.ScheduledEnd,
                Customers = job.Customers.Select(customer => new JobCustomerResponseDTO
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    CompanyId = customer.CompanyId,
                    AddressId = customer.AddressId,

                }).ToList(),
                Storage = new JobStorageResponseDTO
                {
                    StorageId = job.StorageId,
                    StorageItems = job.Storage!.StorageItems.Select(si => new JobStorageItemReponseDTO
                    {
                        StorageItemId = si.StorageItemId,
                        Note = si.Note,
                        ScheduledStart = si.ScheduledStart,
                        ScheduledEnd = si.ScheduledEnd,
                    }).ToList()
                }
            };
        }

        private Job MapJobRequestToJob(JobRequestDTO jobRequest)
        {
            return new Job
            {
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                ScheduledStart = jobRequest.ScheduledStart,
                ScheduledEnd = jobRequest.ScheduledEnd,
                CompanyId = jobRequest.CompanyId,
                AddressId = jobRequest.AddressId
            };
        }
    }
}
