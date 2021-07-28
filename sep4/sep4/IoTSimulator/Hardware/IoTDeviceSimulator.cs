using sep4.IoTSimulator;
using sep4.IoTSimulator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Hardware
{
    public class IoTDeviceSimulator
    {
        private int saunaId;
        private int datapointId;
        private bool isDoorOpen;

        public static float maximumTempValue;
        public static float minimumTempValue;
        public static float maximumCO2Value;
        public static float minimumCO2Value;
        public static float maximumHumValue;
        public static float minimumHumValue;

        public IoTDeviceSimulator(int SaunaId, int DatapointId)
        {
            maximumTempValue = 100;
            minimumTempValue = 0;
            maximumCO2Value = 12800;
            minimumCO2Value = 0;
            maximumHumValue = 1;
            minimumHumValue = 0;
            saunaId = SaunaId;
            datapointId = DatapointId;
            isDoorOpen = false;
        }

        public int getDatapointId()
        {
            return datapointId;
        }

        public int getSaunaId()
        {
            return saunaId;
        }

        public string uplinkDataJson()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;

            float tmp = LastTemp();
            float CO2 = LastCO2();
            float humidity = LastHumidity();

            DataPoint dataPoint = new DataPoint(saunaId, datapointId, DateTime.Now, tmp, CO2, humidity, isDoorOpen);

            string dataJson = JsonConvert.SerializeObject(dataPoint);

            UplinkDataFormat uplinkData = new UplinkDataFormat("rx", string.Parse(datapointId), secondsSinceEpoch, false, 1, 1, dataJson);
            
            string uplinkJson = JsonConvert.SerializeObject(uplinkData);

            return uplinkJson;
        }

        public void controlTheDoor(string downlinkJson)
        {
            DownlinkDataFormat downlinkData = JsonSerializer.Deserialize<DownlinkDataFormat>(downlinkJson);
            string data = downlinkData.getData();
            DoorBooleanFormat doorOpenFormat = JsonSerializer.Deserialize<DoorBooleanFormat>(data);

            isDoorOpen = doorOpenFormat.getDoorOpen();

            if (isDoorOpen)
            {
                Log.printOut("the door is open");
            }
            else
            {
                Log.printOut("the door is closed");
            }
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
            float randomNumber = RNG();
            return randomNumber * (maximum - minimum) + minimum;
        }

        private readonly object lockRoot = new object();
        private float RNG()
        {
            lock (lockRoot)
            {
                Random rng = new Random();
                float randomFloat = (float)rng.NextDouble();
                return randomFloat;
            }
        }

    }
}
