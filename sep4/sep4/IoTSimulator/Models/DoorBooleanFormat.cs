using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DoorBooleanFormat
    {
        private bool isDoorOpen;

        public DoorBooleanFormat(bool isOpen)
        {
            this.isDoorOpen = isOpen;
        }

        public bool getDoorOpen()
        {
            return isDoorOpen;
        }


    }
}
