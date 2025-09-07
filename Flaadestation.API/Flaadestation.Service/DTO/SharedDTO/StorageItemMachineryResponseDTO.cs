using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemMachineryResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public static StorageItemMachineryResponseDTO MapStorageItemMechineryToResponse(StorageItem storageItem)
        {
            if (storageItem.Item is Machinery machinery)
            {
                return new StorageItemMachineryResponseDTO
                {
                    ItemId = storageItem.ItemId,
                    StorageItemId = storageItem.StorageItemId,
                    ScheduledStart = storageItem.ScheduledStart,
                    ScheduledEnd = storageItem.ScheduledEnd,
                    Name = machinery.Name,
                    StorageItemNote = storageItem.Note,
                    ItemNote = machinery.Note,
                    Image = machinery.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = machinery.Image.ImageId,
                        Value = machinery.Image.Value,
                    }
                };
            }
            else
                throw new ArgumentException("Only use this mapping method for Machines!");
        }
    }
}
