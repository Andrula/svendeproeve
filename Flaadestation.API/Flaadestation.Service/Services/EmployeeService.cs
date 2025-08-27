using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.EmployeeDTO;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ApplicationDBContext _context;

        public EmployeeService(IEmployeeRepository employeeRepository, IStorageRepository storageRepository, ApplicationDBContext context)
        {
            _employeeRepository = employeeRepository;
            _storageRepository = storageRepository;
            _context = context;
        }

        public async Task<EmployeeResponseDTO> CreateEmployeeAsync(EmployeeRequestDTO employeeRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var employee = MapEmployeeRequestToEmployee(employeeRequest);

                var createdEmployee = await _employeeRepository.AddAsync(employee);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapEmployeeToEmployeeResponse(createdEmployee);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(Guid employeeId)
        {
            var employeeEntity = await _employeeRepository.GetByIdAsync(employeeId);
            if (employeeEntity == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _employeeRepository.Delete(employeeId);

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

        public async Task<EmployeeResponseDTO?> GetEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            return employee is null ? null : MapEmployeeToEmployeeResponse(employee);
        }

        public async Task<IEnumerable<EmployeeResponseDTO>> GetEmployeesByCompanyAsync(Guid companyId)
        {
            var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);
            return employees.Select(MapEmployeeToEmployeeResponse);
        }

        public async Task<EmployeeResponseDTO?> UpdateEmployeeAsync(Guid employeeId, EmployeeRequestDTO employeeRequest)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
            if (existingEmployee == null)
                return null;

            existingEmployee.FirstName = employeeRequest.FirstName;
            existingEmployee.LastName = employeeRequest.LastName;
            existingEmployee.Email = employeeRequest.Email;
            existingEmployee.Phone = employeeRequest.Phone;
            existingEmployee.OccupationId = employeeRequest.OccupationId;
            existingEmployee.Note = employeeRequest.Note;
            existingEmployee.VehicleId = employeeRequest.VehicleId;
            existingEmployee.DefaultStorageId = employeeRequest.DefaultStorageId;

            _employeeRepository.Update(existingEmployee);
            await _employeeRepository.SaveChangesAsync();
            return MapEmployeeToEmployeeResponse(existingEmployee);
        }

        private EmployeeResponseDTO MapEmployeeToEmployeeResponse(Employee employee)
        {
            var employeeResponse = new EmployeeResponseDTO
            {
                ItemId = employee.ItemId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                Occupation = employee.Occupation is null ? null : new EmployeeOccupationResponseDTO
                {
                    OccupationId = employee.Occupation.OccupationId,
                    Name = employee.Occupation.Name
                },
                Note = employee.Note,
                DefaultStorage = employee.DefaultStorage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(employee.DefaultStorage),
                CompanyId = employee.CompanyId,
                ImageId = employee.ImageId,
                ImageValue = employee.Image?.Value,
                StorageItems = employee.StorageItems.Select(si => new StorageItemResponseDTO
                {
                    StorageItemId = si.StorageItemId,
                    Storage = StorageResponseDTO.MapStorageToStorageResponseDTO(si.Storage!),
                    ScheduledStart = si.ScheduledStart,
                    ScheduledEnd = si.ScheduledEnd,
                    Note = si.Note,
                }).ToList(),
                Vehicle = employee.Vehicle is null ? null : new EmployeeVehicleResponseDTO
                {
                    VehicleId = employee.Vehicle.ItemId,
                    Model = employee.Vehicle.Model,
                    LicensePlate = employee.Vehicle.LicensePlate,
                    Note = employee.Vehicle.Note,
                    ImageId = employee.Vehicle.ImageId,
                    ImageValue = employee.Vehicle.Image?.Value
                }
            };

            if (employee.Vehicle != null)
            {
                var vehicleActiveStorage = employee.Vehicle.StorageItems.FirstOrDefault(si => si.ScheduledStart <= DateTime.Now && si.ScheduledEnd >= DateTime.Now)?.Storage ?? null;

                if (vehicleActiveStorage != null)
                    employeeResponse.Vehicle!.Storage = StorageResponseDTO.MapStorageToStorageResponseDTO(vehicleActiveStorage);
            }

            return employeeResponse;
        }

        private Employee MapEmployeeRequestToEmployee(EmployeeRequestDTO employeeRequest)
        {
            return new Employee
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Phone = employeeRequest.Phone,
                OccupationId = employeeRequest.OccupationId,
                Note = employeeRequest.Note,
                VehicleId = employeeRequest.VehicleId,
                DefaultStorageId = employeeRequest.DefaultStorageId,
                CompanyId = employeeRequest.CompanyId,
            };
        }
    }
}