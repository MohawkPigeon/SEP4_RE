using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models
{
    public class DatapointDTO
    {
        public DatapointDTO(int datapointID, int saunaID, DateTime? dateTime, string temperature, string co2, string humidity, string servoSettingAtTime)
        {
            DatapointID = datapointID;
            SaunaID = saunaID;
            DateTime = dateTime;
            Temperature = temperature;
            Co2 = co2;
            Humidity = humidity;
            ServoSettingAtTime = servoSettingAtTime;
        }

        public int DatapointID { get; set; }
        public int SaunaID { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Temperature { get; set; }
        public string Co2 { get; set; }
        public string Humidity { get; set; }
        public string ServoSettingAtTime { get; set; }
    }
}