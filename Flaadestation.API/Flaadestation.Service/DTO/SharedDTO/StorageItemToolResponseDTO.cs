using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public static StorageItemToolResponseDTO MapStorageItemToolToResponse(StorageItem storageItem)
        {
            if (storageItem.Item is Tool tool)
            {
                return new StorageItemToolResponseDTO
                {
                    ItemId = storageItem.ItemId,
                    StorageItemId = storageItem.StorageItemId,
                    ScheduledStart = storageItem.ScheduledStart,
                    ScheduledEnd = storageItem.ScheduledEnd,
                    Name = tool.Name,
                    StorageItemNote = storageItem.Note,
                    ItemNote = tool.Note,
                    Image = tool.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = tool.Image.ImageId,
                        Value = tool.Image.Value,
                    }
                };
            }
            else
                throw new ArgumentException("Only use this mapping method for Tools!");
        }
    }
}
