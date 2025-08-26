using Flaadestation.Service.DTO.VehicleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleResponseDTO>> GetVehiclesByCompanyAsync(Guid companyId);
        Task<VehicleResponseDTO?> GetVehicleByIdAsync(Guid vehicleId);
        Task<VehicleResponseDTO> CreateVehicleAsync(VehicleRequestDTO vehicleRequest);
        Task<VehicleResponseDTO?> UpdateVehicleAsync(Guid vehicleId, VehicleRequestDTO vehicleRequest);
        Task<bool> DeleteVehicleAsync(Guid vehicleId);
    }
}
