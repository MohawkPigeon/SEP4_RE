using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sep4.HardwareSimulator;
using sep4.HardwareSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace sep4.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HardwareController : ControllerBase
    {

        private WebSocketClient webSocketClient = new WebSocketClient();

        // GET Hardware/id
        [HttpGet("{id}")]
        public ActionResult<string> GetDataFromSensor(int id)
        {
            string deviceId = string.Parse(id);
            DownlinkDataFormat downlinkData = new DownlinkDataFormat("tx", deviceId, 1, true, "0102AABB");
            string json2Send = JsonConvert.SerializeObject(downlinkData);

            string jsonReceived = webSocketClient.sendDownLink(json);

            UplinkDataFormat uplinkData = JsonSerializer.Deserialize<UplinkDataFormat>(json);
            string data = uplinkData.getdata();

            return data;
        }

    }
}
