using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.BaseDTO;
using Flaadestation.Service.DTO.JobDTO;
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
        public async Task<BaseResponseDTO?> GetBaseByIdAsync(Guid id)
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
        public async Task<BaseResponseDTO?> UpdateBaseAsync(Guid id, BaseRequestDTO baseRequest)
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

            return MapBaseToBaseResponse(existingBase);
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
                BaseId = baseEntity.BaseId,
                Name = baseEntity.Name,
                CompanyId = baseEntity.CompanyId,
                AddressId = baseEntity.AddressId,
                Storage = new BaseStorageResponseDTO
                {
                    StorageId = baseEntity.StorageId,
                }
            };

            var storageItems = baseEntity.Storage?.StorageItems ?? new List<StorageItem>();

            foreach (var storageItem in storageItems)
            {
                if (storageItem.Item is Employee)
                {
                    baseResponse.Storage.Employees.Add(StorageItemEmployeeResponseDTO.MapStorageItemEmployeeToResponse(storageItem));
                }

                else if (storageItem.Item is Tool)
                {
                    baseResponse.Storage.Tools.Add(StorageItemToolResponseDTO.MapStorageItemToolToResponse(storageItem));
                }

                else if (storageItem.Item is Machinery)
                {
                    baseResponse.Storage.Machines.Add(StorageItemMachineryResponseDTO.MapStorageItemMechineryToResponse(storageItem));
                }

                else if (storageItem.Item is Vehicle)
                {
                    baseResponse.Storage.Vehicles.Add(StorageItemVehicleResponseDTO.MapStorageItemVehicleToResponse(storageItem));
                }
            }

            var defaultItems = GetItemsWithThisStorageAsDefaultAndNoCurrentAllocations(baseEntity);

            foreach (var item in defaultItems)
            {
                if (item is Employee employee)
                {
                    baseResponse.Storage.DefaultEmployees.Add(new BaseStorageDefaultEmployeeResponseDTO
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
                    baseResponse.Storage.DefaultTools.Add(new BaseStorageDefaultToolResponseDTO
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
                    baseResponse.Storage.DefaultMachines.Add(new BaseStorageDefaultMachineryResponseDTO
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
                    baseResponse.Storage.DefaultVehicles.Add(new BaseStorageDefaultVehicleResponseDTO
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
                        Employees = vehicle.Employees.Select(employee => new BaseStorageDefaultEmployeeResponseDTO
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
                        Tools = vehicle.Tools.Select(tool => new BaseStorageDefaultToolResponseDTO
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

            return baseResponse;
        }

        private List<Item> GetItemsWithThisStorageAsDefaultAndNoCurrentAllocations(Base baseEntity)
        {
            return baseEntity.Storage!.ItemsWithThisStorageAsDefault
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
