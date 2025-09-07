using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public OccupationResponseDTO? Occupation { get; set; }
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public static StorageItemEmployeeResponseDTO MapStorageItemEmployeeToResponse(StorageItem storageItem)
        {
            if (storageItem.Item is Employee employee)
            {
                return new StorageItemEmployeeResponseDTO
                {
                    ItemId = storageItem.ItemId,
                    StorageItemId = storageItem.StorageItemId,
                    ScheduledStart = storageItem.ScheduledStart,
                    ScheduledEnd = storageItem.ScheduledEnd,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    Occupation = employee.Occupation is null ? null : new OccupationResponseDTO
                    {
                        OccupationId = employee.Occupation.OccupationId,
                        Name = employee.Occupation.Name,
                    },
                    StorageItemNote = storageItem.Note,
                    ItemNote = employee.Note,
                    Image = employee.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = employee.Image.ImageId,
                        Value = employee.Image.Value,
                    }
                };
            }
            else
                throw new ArgumentException("Only use this mapping method for Employees!");
        }
    }
}
