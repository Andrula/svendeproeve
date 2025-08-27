using Flaadestation.Repository.Database;
using Flaadestation.Repository.Database.Entities;
using Flaadestation.Repository.Repositories;
using Flaadestation.Repository.Repositories.Interfaces;
using Flaadestation.Service.DTO.SharedDTO;
using Flaadestation.Service.DTO.StorageItemDTO;
using Flaadestation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.Services
{
    public class StorageItemService : IStorageItemService
    {
        private readonly IStorageItemRepository _storageItemRepository;
        private readonly ApplicationDBContext _context;

        public StorageItemService(IStorageItemRepository storageItemRepository, ApplicationDBContext context)
        {
            _storageItemRepository = storageItemRepository;
            _context = context;
        }

        public async Task<StorageItemResponseDTO> CreateStorageItemWithConflictDeleteAsync(StorageItemRequestDTO storageItemRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Delete conflicting storage items
                await DeleteConflictingStorageItemsAsync(
                    storageItemRequest.ItemId,
                    storageItemRequest.ScheduledStart,
                    storageItemRequest.ScheduledEnd);

                // Create the new storage item
                var storageItem = MapStorageItemRequestToStorageItem(storageItemRequest);
                var createdStorageItem = await _storageItemRepository.AddAsync(storageItem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapStorageItemToStorageItemResponse(createdStorageItem);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<StorageItemResponseDTO> CreateStorageItemWithConflictUpdateAsync(StorageItemRequestDTO storageItemRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update conflicting storage items to accommodate the new one
                await UpdateConflictingStorageItemsAsync(
                    storageItemRequest.ItemId,
                    storageItemRequest.ScheduledStart,
                    storageItemRequest.ScheduledEnd);

                // Create the new storage item
                var storageItem = MapStorageItemRequestToStorageItem(storageItemRequest);
                var createdStorageItem = await _storageItemRepository.AddAsync(storageItem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapStorageItemToStorageItemResponse(createdStorageItem);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<StorageItemResponseDTO?> UpdateStorageItemWithConflictDeleteAsync(Guid storageItemId, StorageItemRequestDTO storageItemRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingStorageItem = await _storageItemRepository.GetByIdAsync(storageItemId);
                if (existingStorageItem == null)
                    return null;

                // Delete conflicting storage items (excluding the one being updated)
                await DeleteConflictingStorageItemsAsync(
                    storageItemRequest.ItemId,
                    storageItemRequest.ScheduledStart,
                    storageItemRequest.ScheduledEnd,
                    storageItemId);

                // Update the existing storage item
                existingStorageItem.ScheduledStart = storageItemRequest.ScheduledStart;
                existingStorageItem.ScheduledEnd = storageItemRequest.ScheduledEnd;
                existingStorageItem.Note = storageItemRequest.Note;
                existingStorageItem.ItemId = storageItemRequest.ItemId;
                existingStorageItem.StorageId = storageItemRequest.StorageId;

                _storageItemRepository.Update(existingStorageItem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapStorageItemToStorageItemResponse(existingStorageItem);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<StorageItemResponseDTO?> UpdateStorageItemWithConflictUpdateAsync(Guid storageItemId, StorageItemRequestDTO storageItemRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingStorageItem = await _storageItemRepository.GetByIdAsync(storageItemId);
                if (existingStorageItem == null)
                    return null;

                // Update conflicting storage items (excluding the one being updated)
                await UpdateConflictingStorageItemsAsync(
                    storageItemRequest.ItemId,
                    storageItemRequest.ScheduledStart,
                    storageItemRequest.ScheduledEnd,
                    storageItemId);

                // Update the existing storage item
                existingStorageItem.ScheduledStart = storageItemRequest.ScheduledStart;
                existingStorageItem.ScheduledEnd = storageItemRequest.ScheduledEnd;
                existingStorageItem.Note = storageItemRequest.Note;
                existingStorageItem.ItemId = storageItemRequest.ItemId;
                existingStorageItem.StorageId = storageItemRequest.StorageId;

                _storageItemRepository.Update(existingStorageItem);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapStorageItemToStorageItemResponse(existingStorageItem);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteStorageItemAsync(Guid storageItemId)
        {
            var storageItemEntity = await _storageItemRepository.GetByIdAsync(storageItemId);
            if (storageItemEntity == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                _storageItemRepository.Delete(storageItemId);

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

        public async Task<StorageItemResponseDTO?> GetStorageItemByIdAsync(Guid storageItemId)
        {
            var storageItem = await _storageItemRepository.GetByIdAsync(storageItemId);
            return storageItem is null ? null : MapStorageItemToStorageItemResponse(storageItem);
        }

        public async Task<IEnumerable<StorageItemResponseDTO>> GetStorageItemsByItemIdAsync(Guid itemId)
        {
            var storageItems = await _storageItemRepository.GetStorageItemsByItemIdAsync(itemId);
            return storageItems.Select(MapStorageItemToStorageItemResponse);
        }

        public async Task<IEnumerable<StorageItemResponseDTO>> GetStorageItemsByStorageIdAsync(Guid storageId)
        {
            var storageItems = await _storageItemRepository.GetStorageItemsByStorageIdAsync(storageId);
            return storageItems.Select(MapStorageItemToStorageItemResponse);
        }

        private StorageItemResponseDTO MapStorageItemToStorageItemResponse(StorageItem storageItem)
        {
            return new StorageItemResponseDTO
            {
                StorageItemId = storageItem.StorageItemId,
                ScheduledStart = storageItem.ScheduledStart,
                ScheduledEnd = storageItem.ScheduledEnd,
                Note = storageItem.Note,
                ItemId = storageItem.ItemId,
                Storage = storageItem.Storage is null ? null : StorageResponseDTO.MapStorageToStorageResponseDTO(storageItem.Storage),
            };
        }

        private StorageItem MapStorageItemRequestToStorageItem(StorageItemRequestDTO storageItemRequest)
        {
            return new StorageItem
            {
                ScheduledStart = storageItemRequest.ScheduledStart,
                ScheduledEnd = storageItemRequest.ScheduledEnd,
                Note = storageItemRequest.Note,
                ItemId = storageItemRequest.ItemId,
                StorageId = storageItemRequest.StorageId,
            };
        }

        // Helper methods for conflict resolution
        private async Task DeleteConflictingStorageItemsAsync(Guid itemId, DateTime start, DateTime end, Guid? excludeStorageItemId = null)
        {
            var conflictingItems = await GetConflictingStorageItemsAsync(itemId, start, end, excludeStorageItemId);

            foreach (var conflictingItem in conflictingItems)
            {
                _storageItemRepository.Delete(conflictingItem.StorageItemId);
            }
        }

        private async Task UpdateConflictingStorageItemsAsync(Guid itemId, DateTime start, DateTime end, Guid? excludeStorageItemId = null)
        {
            var conflictingItems = await GetConflictingStorageItemsAsync(itemId, start, end, excludeStorageItemId);

            foreach (var conflictingItem in conflictingItems)
            {
                var conflictType = GetConflictType(conflictingItem, start, end);

                switch (conflictType)
                {
                    case TimespanConflictType.CompleteOverlap:
                        // Delete the conflicting item completely
                        _storageItemRepository.Delete(conflictingItem.StorageItemId);
                        break;

                    case TimespanConflictType.StartOverlap:
                        // Adjust the end time of the conflicting item
                        conflictingItem.ScheduledEnd = start.AddMilliseconds(-1);
                        _storageItemRepository.Update(conflictingItem);
                        break;

                    case TimespanConflictType.EndOverlap:
                        // Adjust the start time of the conflicting item
                        conflictingItem.ScheduledStart = end.AddMilliseconds(1);
                        _storageItemRepository.Update(conflictingItem);
                        break;

                    case TimespanConflictType.MiddleOverlap:
                        // Split the conflicting item into two parts
                        var originalEnd = conflictingItem.ScheduledEnd;

                        // Adjust the first part
                        conflictingItem.ScheduledEnd = start.AddMilliseconds(-1);
                        _storageItemRepository.Update(conflictingItem);

                        // Create the second part
                        var secondPart = new StorageItem
                        {
                            StorageItemId = Guid.NewGuid(),
                            ScheduledStart = end.AddMilliseconds(1),
                            ScheduledEnd = originalEnd,
                            Note = conflictingItem.Note,
                            ItemId = conflictingItem.ItemId,
                            StorageId = conflictingItem.StorageId
                        };
                        await _storageItemRepository.AddAsync(secondPart);
                        break;
                }
            }
        }

        private async Task<IEnumerable<StorageItem>> GetConflictingStorageItemsAsync(Guid itemId, DateTime start, DateTime end, Guid? excludeStorageItemId = null)
        {
            var allStorageItems = await _storageItemRepository.GetStorageItemsByItemIdAsync(itemId);

            return allStorageItems.Where(si =>
                (excludeStorageItemId == null || si.StorageItemId != excludeStorageItemId) &&
                DoTimePeriodsOverlap(si.ScheduledStart, si.ScheduledEnd, start, end));
        }

        private static bool DoTimePeriodsOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 <= end2 && start2 <= end1;
        }

        private static TimespanConflictType GetConflictType(StorageItem existingItem, DateTime newStart, DateTime newEnd)
        {
            var existingStart = existingItem.ScheduledStart;
            var existingEnd = existingItem.ScheduledEnd;

            if (newStart <= existingStart && newEnd >= existingEnd)
                return TimespanConflictType.CompleteOverlap;

            if (newStart <= existingStart && newEnd < existingEnd)
                return TimespanConflictType.StartOverlap;

            if (newStart > existingStart && newEnd >= existingEnd)
                return TimespanConflictType.EndOverlap;

            return TimespanConflictType.MiddleOverlap;
        }
    }
}
