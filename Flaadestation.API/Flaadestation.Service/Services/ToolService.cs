using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.DTO.ToolDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly ApplicationDBContext _context;

        public ToolService(IToolRepository toolRepository, ApplicationDBContext context)
        {
            _toolRepository = toolRepository;
            _context = context;
        }

        public async Task<ToolResponseDTO> CreateToolAsync(ToolRequestDTO toolRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var tool = MapToolRequestToTool(toolRequest);

                var createdTool = await _toolRepository.AddAsync(tool);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapToolToToolResponse(createdTool);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteToolAsync(Guid toolId)
        {
            var toolEntity = await _toolRepository.GetByIdAsync(toolId);
            if (toolEntity == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                _toolRepository.Delete(toolId);

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

        public async Task<ToolResponseDTO?> GetToolByIdAsync(Guid toolId)
        {
            var tool = await _toolRepository.GetByIdAsync(toolId);
            return tool is null ? null : MapToolToToolResponse(tool);
        }

        public async Task<IEnumerable<ToolResponseDTO>> GetToolsByCompanyAsync(Guid companyId)
        {
            var tools = await _toolRepository.GetToolsByCompanyIdAsync(companyId);
            return tools.Select(MapToolToToolResponse);
        }

        public async Task<ToolResponseDTO?> UpdateToolAsync(Guid toolId, ToolRequestDTO toolRequest)
        {
            var existingTool = await _toolRepository.GetByIdAsync(toolId);
            if (existingTool == null)
                return null;

            existingTool.Name = toolRequest.Name;
            existingTool.Note = toolRequest.Note;
            existingTool.VehicleId = toolRequest.VehicleId;
            existingTool.DefaultStorageId = toolRequest.DefaultStorageId;

            _toolRepository.Update(existingTool);
            await _toolRepository.SaveChangesAsync();
            return MapToolToToolResponse(existingTool);
        }

        private ToolResponseDTO MapToolToToolResponse(Tool tool)
        {
            var toolResponse = new ToolResponseDTO
            {
                ItemId = tool.ItemId,
                Name = tool.Name,
                Note = tool.Note,
                DefaultStorage = tool.DefaultStorage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(tool.DefaultStorage),
                CompanyId = tool.CompanyId,
                ImageId = tool.ImageId,
                ImageValue = tool.Image?.Value,
                StorageItems = tool.StorageItems.Select(si => new StorageItemResponseDTO
                {
                    StorageItemId = si.StorageItemId,
                    Storage = StorageResponseDTO.MapStorageToStorageResponseDTO(si.Storage!),
                    ScheduledStart = si.ScheduledStart,
                    ScheduledEnd = si.ScheduledEnd,
                    Note = si.Note,
                    ItemId = si.ItemId,
                }).ToList(),
                Vehicle = tool.Vehicle is null ? null : new ToolVehicleResponseDTO
                {
                    VehicleId = tool.Vehicle.ItemId,
                    Model = tool.Vehicle.Model,
                    LicensePlate = tool.Vehicle.LicensePlate,
                    Note = tool.Vehicle.Note,
                    ImageId = tool.Vehicle.ImageId,
                    ImageValue = tool.Vehicle.Image?.Value
                }
            };

            if (tool.Vehicle != null)
            {
                var vehicleActiveStorage = tool.Vehicle.StorageItems.FirstOrDefault(si => si.ScheduledStart <= DateTime.Now && si.ScheduledEnd >= DateTime.Now)?.Storage ?? null;

                if (vehicleActiveStorage != null)
                    toolResponse.Vehicle!.Storage = StorageResponseDTO.MapStorageToStorageResponseDTO(vehicleActiveStorage);
            }

            return toolResponse;
        }

        private Tool MapToolRequestToTool(ToolRequestDTO toolRequest)
        {
            return new Tool
            {
                Name = toolRequest.Name,
                Note = toolRequest.Note,
                VehicleId = toolRequest.VehicleId,
                DefaultStorageId = toolRequest.DefaultStorageId,
                CompanyId = toolRequest.CompanyId,
            };
        }
    }
}
