using Flaadestation.Repository.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Flaadestation.Shared.Constants;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class StorageResponseDTO
    {
        /// <summary>
        /// ID på Job eller Base
        /// </summary>
        public Guid RelevantId { get; set; }
        public StorageType StorageType { get; set; }
        /// <summary>
        /// Name (Base) eller Title (Job)
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public Guid StorageId { get; set; }
        public Guid AddressId { get; set; }

        public static StorageResponseDTO MapStorageToStorageResponseDTO(Storage storage)
        {
            StorageType storageType = storage.Base is null ? StorageType.Job : StorageType.Base;

            return new StorageResponseDTO
            {
                StorageId = storage.StorageId,
                RelevantId = storageType == StorageType.Base ? storage.Base!.BaseId : storage.Job!.JobId,
                StorageType = storageType,
                Name = storageType == StorageType.Base ? storage.Base!.Name : storage.Job!.Title,
                AddressId = storageType == StorageType.Base ? storage.Base!.AddressId : storage.Job!.AddressId,
            };
        }
    }


}
