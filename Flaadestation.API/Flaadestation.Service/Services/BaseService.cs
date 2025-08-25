using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.BaseDTO;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class BaseService : IBaseService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ApplicationDBContext _context;

        public BaseService(IBaseRepository baseRepository, IStorageRepository storageRepositor, ApplicationDBContext context)
        {
            _baseRepository = baseRepository;
            _storageRepository = storageRepositor;
            _context = context;
        }

        // Metode til at oprette en base med et tilknyttet storage entitet.
        public async Task<BaseResponseDTO> CreateBaseAsync(BaseRequestDTO baseRequest)
        {
            var nameExists = await _baseRepository.BaseExistsByNameAsync(baseRequest.Name, baseRequest.CompanyId);
            if (nameExists)
            {
                throw new InvalidOperationException($"Base med navnet '{baseRequest.Name}' eksisterer allerede for denne virksomhed.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var storage = new Storage();

                await _storageRepository.AddAsync(storage);

                var createdBase = MapBaseRequestToBase(baseRequest);

                createdBase.StorageId = storage.StorageId;
                
                var mappedBase =  await _baseRepository.AddAsync(createdBase); 

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapBaseToBaseResponse(mappedBase);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// Metode til at slette base.
        public async Task<bool> DeleteBaseAsync(Guid id)
        {
            var baseEntity = await _baseRepository.GetBaseWithStorageAsync(id);
            if (baseEntity == null)
                return false;

            if (baseEntity.Storage?.StorageItems.Any() == true)
            {
                throw new InvalidOperationException("Kan ikke slette base der har ting opbevaret. Flyt alle items først.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (baseEntity.Storage != null)
                {
                     _storageRepository.Delete(baseEntity.Storage.StorageId);
                }

                _baseRepository.Delete(id);

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

        // Metode til at hente base via. ID.
        public async Task<BaseResponseDTO> GetBaseByIdAsync(Guid id)
        {
            var baseEntity = await _baseRepository.GetByIdAsync(id);
            return baseEntity is null ? null : MapBaseToBaseResponse(baseEntity);
        }

        // Metode til at hente base med en tilknyttet storage.
        public async Task<Base?> GetBaseWithStorageAsync(Guid baseId)
        {
            return await _baseRepository.GetBaseWithStorageAsync(baseId);
        }

        // Metode til at hente virksomhedens baser.
        public async Task<IEnumerable<BaseResponseDTO>> GetBasesByCompanyAsync(Guid companyId)
        {
            var baseEntities = await _baseRepository.GetBasesByCompanyAsync(companyId);
            return baseEntities.Select(MapBaseToBaseResponse);
        }

        // Metode til at om navn på base allerede eksisterer.
        public async Task<bool> IsBaseNameAvailableAsync(string name, Guid companyId)
        {
            return !await _baseRepository.BaseExistsByNameAsync(name, companyId);
        }

        // Metode til at opdatere informmationer på base.
        public async Task<Base?> UpdateBaseAsync(Guid id, BaseRequestDTO baseRequest)
        {
            var existingBase = await _baseRepository.GetByIdAsync(id);
            if (existingBase == null)
                return null;

            if (existingBase.Name != baseRequest.Name)
            {
                var nameExists = await _baseRepository.BaseExistsByNameAsync(baseRequest.Name, existingBase.CompanyId);
                if (nameExists)
                {
                    throw new InvalidOperationException($"Base med navnet '{baseRequest.Name}' eksisterer allerede for denne virksomhed.");
                }
            }

            existingBase.Name = baseRequest.Name;
            existingBase.AddressId = baseRequest.AddressId;
            _baseRepository.Update(existingBase);
            await _baseRepository.SaveChangesAsync();
            return existingBase;
        }

        private Base MapBaseRequestToBase(BaseRequestDTO request)
        {
            return new Base
            {
                Name = request.Name,
                CompanyId = request.CompanyId,
                AddressId = request.AddressId
            };
        }

        private BaseResponseDTO MapBaseToBaseResponse(Base baseEntity)
        {
            var baseResponse =  new BaseResponseDTO
            {
                Name = baseEntity.Name,
                CompanyId = baseEntity.CompanyId,
                AddressId = baseEntity.AddressId,
                Employees = baseEntity.Employees.Select(employee => new BaseEmployeeResponseDTO
                {
                    EmployeeId = employee.ItemId,
                    Name = (employee.FirstName + employee.LastName),
                    Email = employee.Email,
                    Phone = employee.Phone,
                    CompanyId = employee.CompanyId
                   
                }).ToList(),
                Storage = new BaseStorageResponseDTO
                {
                    StorageId = baseEntity.StorageId,
                }
            };

            foreach (var storageItem in baseEntity.Storage!.StorageItems)
            {
                if (storageItem.Item is Employee employee)
                {
                    baseResponse.Storage.Employees.Add(new StorageItemEmployeeResponseDTO
                    {
                        ItemId = storageItem.ItemId,
                        StorageItemId = storageItem.StorageItemId,
                        ScheduledStart = storageItem.ScheduledStart,
                        ScheduledEnd = storageItem.ScheduledEnd,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        Phone = employee.Phone,
                        OccupationId = employee.OccupationId,
                        Occupation = employee.Occupation is null ? "" : employee.Occupation.Name,
                        StorageItemNote = storageItem.Note,
                        ItemNote = employee.Note,
                        ImageId = employee.ImageId,
                        ImageValue = employee.Image is null ? null : employee.Image.Value
                    });
                }

                else if (storageItem.Item is Tool tool)
                {
                    baseResponse.Storage.Tools.Add(new StorageItemToolResponseDTO
                    {
                        ItemId = storageItem.ItemId,
                        StorageItemId = storageItem.StorageItemId,
                        ScheduledStart = storageItem.ScheduledStart,
                        ScheduledEnd = storageItem.ScheduledEnd,
                        Name = tool.Name,
                        StorageItemNote = storageItem.Note,
                        ItemNote = tool.Note,
                        ImageId = tool.ImageId,
                        ImageValue = tool.Image is null ? null : tool.Image.Value
                    });
                }

                else if (storageItem.Item is Machinery machine)
                {
                    baseResponse.Storage.Machines.Add(new StorageItemMachineryResponseDTO
                    {
                        ItemId = storageItem.ItemId,
                        StorageItemId = storageItem.StorageItemId,
                        ScheduledStart = storageItem.ScheduledStart,
                        ScheduledEnd = storageItem.ScheduledEnd,
                        Name = machine.Name,
                        StorageItemNote = storageItem.Note,
                        ItemNote = machine.Note,
                        ImageId = machine.ImageId,
                        ImageValue = machine.Image is null ? null : machine.Image.Value
                    });
                }

                else if (storageItem.Item is Vehicle vehicle)
                {
                    baseResponse.Storage.Vehicles.Add(new StorageItemVehicleResponseDTO
                    {
                        ItemId = storageItem.ItemId,
                        StorageItemId = storageItem.StorageItemId,
                        ScheduledStart = storageItem.ScheduledStart,
                        ScheduledEnd = storageItem.ScheduledEnd,
                        Model = vehicle.Model,
                        LicensePlate = vehicle.LicensePlate,
                        StorageItemNote = storageItem.Note,
                        ItemNote = vehicle.Note,
                        ImageId = vehicle.ImageId,
                        ImageValue = vehicle.Image is null ? null : vehicle.Image.Value,
                        Employees = vehicle.Employees.Select(employee => new StorageItemVehicleEmployeeResponseDTO
                        {
                            ItemId = employee.ItemId,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            Phone = employee.Phone,
                            OccupationId = employee.OccupationId,
                            Occupation = employee.Occupation is null ? "" : employee.Occupation.Name,
                            Note = employee.Note,
                            ImageId = employee.ImageId,
                            ImageValue = employee.Image is null ? null : employee.Image.Value
                        }).ToList(),
                        Tools = vehicle.Tools.Select(tool => new StorageItemVehicleToolResponseDTO
                        {
                            ItemId = storageItem.ItemId,
                            Name = tool.Name,
                            Note = tool.Note,
                            ImageId = tool.ImageId,
                            ImageValue = tool.Image is null ? null : tool.Image.Value
                        }).ToList(),
                    });
                }
            }

            return baseResponse;
        }
    }
}
