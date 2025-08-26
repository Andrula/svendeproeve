using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.DTO.VehicleDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ApplicationDBContext _context;

        public VehicleService(IVehicleRepository vehicleRepository, IStorageRepository storageRepository, ApplicationDBContext context)
        {
            _vehicleRepository = vehicleRepository;
            _storageRepository = storageRepository;
            _context = context;
        }

        public async Task<VehicleResponseDTO> CreateVehicleAsync(VehicleRequestDTO vehicleRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var vehicle = MapVehicleRequestToVehicle(vehicleRequest);

                var createdVehicle = await _vehicleRepository.AddAsync(vehicle);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapVehicleToVehicleResponse(createdVehicle);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteVehicleAsync(Guid vehicleId)
        {
            var vehicleEntity = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicleEntity == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                _vehicleRepository.Delete(vehicleId);

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

        public async Task<VehicleResponseDTO?> GetVehicleByIdAsync(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            return vehicle is null ? null : MapVehicleToVehicleResponse(vehicle);
        }

        public async Task<IEnumerable<VehicleResponseDTO>> GetVehiclesByCompanyAsync(Guid companyId)
        {
            var vehicles = await _vehicleRepository.GetVehiclesByCompanyIdAsync(companyId);
            return vehicles.Select(MapVehicleToVehicleResponse);
        }

        public async Task<VehicleResponseDTO?> UpdateVehicleAsync(Guid vehicleId, VehicleRequestDTO vehicleRequest)
        {
            var existingVehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (existingVehicle == null)
                return null;

            existingVehicle.Model = vehicleRequest.Model;
            existingVehicle.LicensePlate = vehicleRequest.LicensePlate;
            existingVehicle.Note = vehicleRequest.Note;
            existingVehicle.DefaultStorageId = vehicleRequest.DefaultStorageId;

            _vehicleRepository.Update(existingVehicle);
            await _vehicleRepository.SaveChangesAsync();
            return MapVehicleToVehicleResponse(existingVehicle);
        }

        private VehicleResponseDTO MapVehicleToVehicleResponse(Vehicle vehicle)
        {
            return new VehicleResponseDTO
            {
                ItemId = vehicle.ItemId,
                Model = vehicle.Model,
                LicensePlate = vehicle.LicensePlate,
                Note = vehicle.Note,
                DefaultStorage = vehicle.DefaultStorage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(vehicle.DefaultStorage),
                CompanyId = vehicle.CompanyId,
                Image = vehicle.Image is null ? null : new ImageResponseDTO
                {
                    ImageId = vehicle.Image.ImageId,
                    Value = vehicle.Image.Value,
                },
                StorageItems = vehicle.StorageItems.Select(si => new StorageItemResponseDTO
                {
                    StorageItemId = si.ItemId,
                    Note = si.Note,
                    ScheduledStart = si.ScheduledStart,
                    ScheduledEnd = si.ScheduledEnd,
                    Storage = si.Storage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(si.Storage)
                }).ToList(),
                Employees = vehicle.Employees.Select(e => new VehicleEmployeeResponseDTO
                {
                    ItemId = e.ItemId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Note = e.Note,
                    Image = e.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = e.Image.ImageId,
                        Value = e.Image.Value,
                    },
                    Occupation = e.Occupation is null ? null : new OccupationResponseDTO
                    {
                        OccupationId = e.Occupation.OccupationId,
                        Name = e.Occupation.Name,
                    }
                }).ToList(),
                Tools = vehicle.Tools.Select(t => new VehicleToolResponseDTO
                {
                    ItemId = t.ItemId,
                    Name = t.Name,
                    Note = t.Note,
                    Image = t.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = t.Image.ImageId,
                        Value = t.Image.Value,
                    }
                }).ToList()
            };
        }

        private Vehicle MapVehicleRequestToVehicle(VehicleRequestDTO vehicleRequest)
        {
            return new Vehicle
            {
                Model = vehicleRequest.Model,
                LicensePlate = vehicleRequest.LicensePlate,
                Note = vehicleRequest.Note,
                DefaultStorageId = vehicleRequest.DefaultStorageId,
                CompanyId = vehicleRequest.CompanyId,
                // TODO: Vi skal have implementeret noget logik til at håndtere Image - Dette gælder for alle items
            };
        }
    }
}
