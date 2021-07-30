using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DataPoint
    {
        private int SaunaId;
        private int DataPointId;
        private DateTime Time;
        private float Temperature;
        private float CO2;
        private float Humidity;
        private bool ServoSetting;

        public DataPoint(int SaunaId, int DataPointId, DateTime time, float temperature, float CO2, float humidity, bool servoSetting)
        {
            this.SaunaId = SaunaId;
            this.DatapointID = DatapointID;
            Time = time;
            Temperature = temperature;
            this.CO2 = CO2;
            Humidity = humidity;
            ServoSetting = servoSetting;
        }


        public float getSaunaId()
        {
            return SaunaId;
        }

        public float getDataPointId()
        {
            return DataPointId;
        }

        public DateTime getDateTime()
        {
            return Time;
        }

        public float getTemperature()
        {
            return Temperature;
        }

        public float getCO2()
        {
            return CO2;
        }

        public float getHumidity()
        {
            return Humidity;
        }

        public bool getServoSetting()
        {
            return ServoSetting;
        }

    }
}
