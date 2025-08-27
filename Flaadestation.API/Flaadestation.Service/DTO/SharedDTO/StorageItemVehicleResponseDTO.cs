using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageItemVehicleResponseDTO
    {
        public Guid ItemId { get; set; }
        public Guid StorageItemId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string StorageItemNote { get; set; } = string.Empty;
        public string ItemNote { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }

        public List<StorageItemVehicleEmployeeResponseDTO> Employees { get; set; } = [];
        public List<StorageItemVehicleToolResponseDTO> Tools { get; set; } = [];

        public static StorageItemVehicleResponseDTO MapStorageItemVehicleToResponse(StorageItem storageItem)
        {
            if (storageItem.Item is Vehicle vehicle)
            {
                return new StorageItemVehicleResponseDTO
                {
                    ItemId = storageItem.ItemId,
                    StorageItemId = storageItem.StorageItemId,
                    ScheduledStart = storageItem.ScheduledStart,
                    ScheduledEnd = storageItem.ScheduledEnd,
                    Model = vehicle.Model,
                    LicensePlate = vehicle.LicensePlate,
                    StorageItemNote = storageItem.Note,
                    ItemNote = vehicle.Note,
                    Image = vehicle.Image is null ? null : new ImageResponseDTO
                    {
                        ImageId = vehicle.Image.ImageId,
                        Value = vehicle.Image.Value,
                    },
                    Employees = vehicle.Employees.Select(employee => new StorageItemVehicleEmployeeResponseDTO
                    {
                        ItemId = employee.ItemId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        Phone = employee.Phone,
                        Occupation = employee.Occupation is null ? null : new OccupationResponseDTO
                        {
                            OccupationId = employee.Occupation.OccupationId,
                            Name = employee.Occupation.Name,
                        },
                        Note = employee.Note,
                        Image = employee.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = employee.Image.ImageId,
                            Value = employee.Image.Value,
                        }
                    }).ToList(),
                    Tools = vehicle.Tools.Select(tool => new StorageItemVehicleToolResponseDTO
                    {
                        ItemId = storageItem.ItemId,
                        Name = tool.Name,
                        Note = tool.Note,
                        Image = tool.Image is null ? null : new ImageResponseDTO
                        {
                            ImageId = tool.Image.ImageId,
                            Value = tool.Image.Value,
                        }
                    }).ToList(),
                };
            }
            else
                throw new ArgumentException("Only use this mapping method for Vehicles!");
        }
    }

    public class StorageItemVehicleEmployeeResponseDTO
    {
        public Guid ItemId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public OccupationResponseDTO? Occupation { get; set; }
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
    }

    public class StorageItemVehicleToolResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ImageResponseDTO? Image { get; set; }
    }
}
