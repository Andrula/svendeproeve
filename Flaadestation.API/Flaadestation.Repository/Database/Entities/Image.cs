using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Repository.Database.Entities
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public required byte[] Value { get; set; }

        public Item? Item { get; set; }
    }
}
