using sep4.HardwareSimulator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.HardwareSimulator
{
    public class DataGenerator
    {
        public static float maximumTempValue;
        public static float minimumTempValue;
        public static float maximumCO2Value;
        public static float minimumCO2Value;
        public static float maximumHumValue;
        public static float minimumHumValue;

        public DataGenerator()
        {
            maximumTempValue = 100;
            minimumTempValue = 0;
            maximumCO2Value = 12800;
            minimumCO2Value = 0;
            maximumHumValue = 1;
            minimumHumValue = 0;
        }

        public string DataJson()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            float tmp = LastTemp();
            float CO2 = LastCO2();
            float humidity = LastHumidity();

            DataPoint dataPoint = new DataPoint(DateTime.Now, tmp, CO2, humidity);

            string dataJson = JsonConvert.SerializeObject(dataPoint);

            UplinkDataFormat uplinkData = new UplinkDataFormat("rx", "001", secondsSinceEpoch, false, 1, 1, dataJson);
            
            string uplinkJson = JsonConvert.SerializeObject(uplinkData);

            return uplinkJson;
        }

        private float LastTemp()
        {
            return GenerateRandomNumberInRange(maximumTempValue, minimumTempValue);
        }

        private float LastCO2()
        {
            return GenerateRandomNumberInRange(maximumCO2Value, minimumCO2Value);
        }

        private float LastHumidity()
        {
            return GenerateRandomNumberInRange(maximumHumValue, minimumHumValue);
        }

        private float GenerateRandomNumberInRange(float maximum, float minimum)
        {
            float randomNumber = RandomNumber.GetInstance().RNG();
            return randomNumber * (maximum - minimum) + minimum;
        }

        

    }
}
