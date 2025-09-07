using Flaadestation.Service.DTO.StorageItemDTO;
using Flaadestation.Service.Interfaces;
using Flaadestation.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flaadestation.ASP.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageItemController : Controller
    {
        private readonly IStorageItemService _storageItemService;

        public StorageItemController(IStorageItemService storageItemService)
        {
            _storageItemService = storageItemService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStorageItem(Guid id)
        {
            var storageItemResponse = await _storageItemService.GetStorageItemByIdAsync(id);
            if (storageItemResponse == null)
                return NotFound();

            return Ok(storageItemResponse);
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetStorageItemsByItem(Guid itemId)
        {
            var storageItems = await _storageItemService.GetStorageItemsByItemIdAsync(itemId);
            return Ok(storageItems);
        }

        [HttpGet("storage/{itemId}")]
        public async Task<IActionResult> GetStorageItemsByStorage(Guid storageId)
        {
            var storageItems = await _storageItemService.GetStorageItemsByStorageIdAsync(storageId);
            return Ok(storageItems);
        }

        [HttpPost("conflict-delete")]
        public async Task<IActionResult> CreateStorageItemWithConflictDelete([FromBody] StorageItemRequestDTO storageItemRequest)
        {
            try
            {
                var created = await _storageItemService.CreateStorageItemWithConflictDeleteAsync(storageItemRequest);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("conflict-update")]
        public async Task<IActionResult> CreateStorageItemWithConflictUpdate([FromBody] StorageItemRequestDTO storageItemRequest)
        {
            try
            {
                var created = await _storageItemService.CreateStorageItemWithConflictUpdateAsync(storageItemRequest);

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("conflict-delete/{id}")]
        public async Task<IActionResult> UpdateStorageItemWithConflictDelete(Guid id, [FromBody] StorageItemRequestDTO storageItemRequest)
        {
            try
            {
                var updated = await _storageItemService.UpdateStorageItemWithConflictDeleteAsync(id, storageItemRequest);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("conflict-update/{id}")]
        public async Task<IActionResult> UpdateStorageItemWithConflictUpdate(Guid id, [FromBody] StorageItemRequestDTO storageItemRequest)
        {
            try
            {
                var updated = await _storageItemService.UpdateStorageItemWithConflictUpdateAsync(id, storageItemRequest);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorageItem(Guid id)
        {
            try
            {
                var deleted = await _storageItemService.DeleteStorageItemAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
