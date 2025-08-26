using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Service.DTO.SharedDTO
{
    public class ImageResponseDTO
    {
        public Guid ImageId { get; set; }
        public required byte[] Value { get; set; }
    }
}
