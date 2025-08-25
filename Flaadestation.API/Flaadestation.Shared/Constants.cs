using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flaadestation.Shared
{
    public static class Constants
    {
        public enum ItemType : int
        {
            Employee = 1,
            Vehicle = 2,
            Tool = 3,
            Machine = 4,
        }

        public enum StorageType : int
        {
            Base = 1,
            Job = 2,
        }
    }
}
