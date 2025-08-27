using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.DTO.StorageItemDTO;
using Flaadestation.Service.DTO.ToolDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.Interfaces
{
    public interface IStorageItemService
    {
        Task<StorageItemResponseDTO?> GetStorageItemByIdAsync(Guid storageItemId);
        Task<IEnumerable<StorageItemResponseDTO>> GetStorageItemsByItemIdAsync(Guid itemId);
        Task<IEnumerable<StorageItemResponseDTO>> GetStorageItemsByStorageIdAsync(Guid storageId);
        Task<StorageItemResponseDTO> CreateStorageItemWithConflictDeleteAsync(StorageItemRequestDTO storageItemRequest);
        Task<StorageItemResponseDTO> CreateStorageItemWithConflictUpdateAsync(StorageItemRequestDTO storageItemRequest);
        Task<StorageItemResponseDTO?> UpdateStorageItemWithConflictDeleteAsync(Guid storageItemId, StorageItemRequestDTO storageItemRequest);
        Task<StorageItemResponseDTO?> UpdateStorageItemWithConflictUpdateAsync(Guid storageItemId, StorageItemRequestDTO storageItemRequest);
        Task<bool> DeleteStorageItemAsync(Guid storageItemId);
    }
}
