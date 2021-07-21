using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class ServoSettingDTO
    {
        public ServoSettingDTO(int servoSettingID, int saunaID, Nullable<System.DateTime> datetime, string servoSetting1)
        {
            ServoSettingID = servoSettingID;
            SaunaID = saunaID;
            Datetime = datetime;
            ServoSetting1 = servoSetting1;
        }

        public int ServoSettingID { get; set; }
        public int SaunaID { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public string ServoSetting1 { get; set; }
    }
}