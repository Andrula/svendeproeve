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

        public enum TimespanConflictType
        {
            CompleteOverlap,    // New period completely covers the existing one
            StartOverlap,       // New period overlaps with the start of existing one
            EndOverlap,         // New period overlaps with the end of existing one
            MiddleOverlap       // New period is in the middle of existing one
        }
    }
}
