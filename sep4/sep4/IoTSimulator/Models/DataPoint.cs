using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DataPoint
    {
        public int SaunaId { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public float CO2 { get; set; }
        public float Humidity { get; set; }
        public bool ServoSetting { get; set; }

        public DataPoint(int saunaId, DateTime time, float temperature, float cO2, float humidity, bool servoSetting)
        {
            SaunaId = saunaId;
            Time = time;
            Temperature = temperature;
            CO2 = cO2;
            Humidity = humidity;
            ServoSetting = servoSetting;
        }
    }
}
