using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DoorBooleanFormat
    {
        public bool isDoorOpen { get; set; }

        public DoorBooleanFormat(bool isDoorOpen)
        {
            this.isDoorOpen = isDoorOpen;
        }
    }
}
