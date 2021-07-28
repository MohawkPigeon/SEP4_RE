using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sep4.IoTSimulator.WebSocket;
using sep4.IoTSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace sep4.Controllers
{
    public class HardwareController : ApiController
    {
        private WebSocketClient webSocketClient = WebSocketClient.getInstance();

        // GET Hardware/id
        [Route("api/openDoor/{id}")]
        [HttpGet]
        public IHttpActionResult OpenTheDoor(int id)
        {
            string deviceId = string.Parse(id);
            bool openDoor = true;

            DoorBooleanFormat data = new DoorBooleanFormat(true);
            string openDoorDataJson = JsonConvert.SerializeObject(data);

            DownlinkDataFormat downlinkData = new DownlinkDataFormat("tx", deviceId, 1, true, "0102AABB", openDoorDataJson);
            string json2Send = JsonConvert.SerializeObject(downlinkData);

            foreach (IoTSimulator simulator in webSocketClient.getAllDevice())
            {
                if (simulator.getDatapointId() == id)
                {
                    webSocketClient.sendDownLink(simulator, json2Send);
                }
            }

            //return success
            return Ok(downlinkData);
        }

        // GET Hardware/id
        [Route("api/closeDoor/{id}")]
        [HttpGet]
        public IHttpActionResult CloseTheDoor(int id)
        {
            string deviceId = string.Parse(id);
            bool openDoor = false;

            DoorBooleanFormat data = new DoorBooleanFormat(true);
            string openDoorDataJson = JsonConvert.SerializeObject(data);

            DownlinkDataFormat downlinkData = new DownlinkDataFormat("tx", deviceId, 1, true, "0102AABB", openDoorDataJson);
            string json2Send = JsonConvert.SerializeObject(downlinkData);

            foreach (IoTSimulator simulator in webSocketClient.getAllDevice())
            {
                if (simulator.getDatapointId() == id)
                {
                    webSocketClient.sendDownLink(simulator, json2Send);
                }
            }

            //return success
            return Ok(downlinkData);
        }


    }
}
