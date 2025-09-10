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
using Flaadestation.Service.DTO.SharedDTO;
using System.Net;

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
            var jobResponse = new JobResponseDTO
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                AddressId = job.AddressId,
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
                    StorageId = job.StorageId
                }
            };

            foreach (var storageItem in job.Storage!.StorageItems)
            {
                if (storageItem.Item is Employee)
                {
                    jobResponse.Storage.Employees.Add(StorageItemEmployeeResponseDTO.MapStorageItemEmployeeToResponse(storageItem));
                }

                else if (storageItem.Item is Tool)
                {
                    jobResponse.Storage.Tools.Add(StorageItemToolResponseDTO.MapStorageItemToolToResponse(storageItem));
                }

                else if (storageItem.Item is Machinery)
                {
                    jobResponse.Storage.Machines.Add(StorageItemMachineryResponseDTO.MapStorageItemMechineryToResponse(storageItem));
                }

                else if (storageItem.Item is Vehicle)
                {
                    jobResponse.Storage.Vehicles.Add(StorageItemVehicleResponseDTO.MapStorageItemVehicleToResponse(storageItem));
                }
            }

            var defaultItems = GetItemsWithThisStorageAsDefaultAndNoCurrentAllocations(job);

            foreach (var item in defaultItems)
            {
                if (item is Employee employee)
                {
                    jobResponse.Storage.DefaultEmployees.Add(new JobStorageDefaultEmployeeResponseDTO
                    {
                        ItemId = employee.ItemId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        Phone = employee.Phone,
                        Note = employee.Note,
                        Image = employee.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = employee.Image.ImageId,
                            Value = employee.Image.Value
                        },
                        Occupation = employee.Occupation is null ? null : new OccupationResponseDTO
                        {
                            OccupationId = employee.Occupation.OccupationId,
                            Name = employee.Occupation.Name,
                        }
                    });
                }

                else if (item is Tool tool)
                {
                    jobResponse.Storage.DefaultTools.Add(new JobStorageDefaultToolResponseDTO
                    {
                        ItemId = tool.ItemId,
                        Note = tool.Note,
                        Name = tool.Name,
                        Image = tool.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = tool.Image.ImageId,
                            Value = tool.Image.Value
                        },
                    });
                }

                else if (item is Machinery machine)
                {
                    jobResponse.Storage.DefaultMachines.Add(new JobStorageDefaultMachineryResponseDTO
                    {
                        ItemId = machine.ItemId,
                        Note = machine.Note,
                        Name = machine.Name,
                        Image = machine.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = machine.Image.ImageId,
                            Value = machine.Image.Value
                        },
                    });
                }

                else if (item is Vehicle vehicle)
                {
                    jobResponse.Storage.DefaultVehicles.Add(new JobStorageDefaultVehicleResponseDTO
                    {
                        ItemId = vehicle.ItemId,
                        Model = vehicle.Model,
                        LicensePlate = vehicle.LicensePlate,
                        Note = vehicle.Note,
                        Image = vehicle.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = vehicle.Image.ImageId,
                            Value = vehicle.Image.Value
                        },
                        Employees = vehicle.Employees.Select(employee => new JobStorageDefaultEmployeeResponseDTO
                        {
                            ItemId = employee.ItemId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            Phone = employee.Phone,
                            Note = employee.Note,
                            Image = employee.Image is null ? null : new ImageResponseDTO
                            {
                                ImageId = employee.Image.ImageId,
                                Value = employee.Image.Value
                            },
                            Occupation = employee.Occupation is null ? null : new OccupationResponseDTO
                            {
                                OccupationId = employee.Occupation.OccupationId,
                                Name = employee.Occupation.Name,
                            }
                        }).ToList(),
                        Tools = vehicle.Tools.Select(tool => new JobStorageDefaultToolResponseDTO
                        {
                            ItemId = tool.ItemId,
                            Note = tool.Note,
                            Name = tool.Name,
                            Image = tool.Image is null ? null : new ImageResponseDTO
                            {
                                ImageId = tool.Image.ImageId,
                                Value = tool.Image.Value
                            },
                        }).ToList(),
                    });
                }
            }

            return jobResponse;
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

        private List<Item> GetItemsWithThisStorageAsDefaultAndNoCurrentAllocations(Job job)
        {
            return job.Storage!.ItemsWithThisStorageAsDefault
                .Where(i =>
                    (
                        (i is Employee e && e.VehicleId != null && e.VehicleId != Guid.Empty)
                        || (i is Tool t && t.VehicleId != null && t.VehicleId != Guid.Empty)
                        || (i is Vehicle)
                        || (i is Machinery)
                    )
                    &&
                    !i.StorageItems.Any(si =>
                        si.ScheduledStart.Date <= DateTime.Today &&
                        si.ScheduledEnd.Date >= DateTime.Today))
                .ToList();
        }
    }
}
