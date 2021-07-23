using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class ServoSettingDTO
    {
        public ServoSettingDTO(int servoSettingID, int saunaID, Nullable<System.DateTime> datetime, string setting)
        {
            ServoSettingID = servoSettingID;
            SaunaID = saunaID;
            Datetime = datetime;
            Setting = setting;
        }

        public int ServoSettingID { get; set; }
        public int SaunaID { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public string Setting { get; set; }
    }
}