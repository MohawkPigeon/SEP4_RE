using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using sep4.IoTSimulator.WebSocket;
using sep4.IoTSimulator.Hardware;
using sep4.IoTSimulator.Models;
using Newtonsoft.Json;


namespace IoTUnitTest
{
    [TestClass]
    public class IoTUnitTest
    {

        [TestMethod]
        public void AddDeviceTest()
        {
            Assert.IsFalse(WebSocketClient.getInstance().addDevice(1));
            Assert.IsTrue(WebSocketClient.getInstance().addDevice(99));
        }

        [TestMethod]
        public void DeleteDeviceTest()
        {
            Assert.IsTrue(WebSocketClient.getInstance().deleteDevice(1));
            Assert.IsFalse(WebSocketClient.getInstance().deleteDevice(999));
        }

        [TestMethod]
        public void ControlTheDoor()
        {
            Assert.ThrowsException<NullReferenceException>(() => new IoTDeviceSimulator(1).controlTheDoor(""));
            Assert.ThrowsException<JsonSerializationException>(() => new IoTDeviceSimulator(1).controlTheDoor("111"));
        }

        [TestMethod]
        public void RecieveUpLink()
        {
            string dataJson = WebSocketClient.getInstance().receiveUplink(new IoTDeviceSimulator(1));

            UplinkDataFormat uplinkData = JsonConvert.DeserializeObject<UplinkDataFormat>(dataJson);
            string data = uplinkData.data;
            DataPoint dataPoint = JsonConvert.DeserializeObject<DataPoint>(data);

            Assert.IsNotNull(dataPoint.SaunaId);
            Assert.IsNotNull(dataPoint.CO2);
            Assert.IsNotNull(dataPoint.Humidity);
            Assert.IsNotNull(dataPoint.Temperature);
            Assert.IsNotNull(dataPoint.Time);
            Assert.IsFalse(dataPoint.ServoSetting);
        }

    }
}

