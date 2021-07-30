using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sep4.IoTSimulator.WebSocket;
using sep4.IoTSimulator.Models;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Description;

namespace sep4.Controllers
{
    public class HardwareController : ApiController
    {
        private WebSocketClient webSocketClient = WebSocketClient.getInstance();

        // GET  api/hardwarecontroller/getopenthedoor/1
        [Route("api/hardwarecontroller/getopenthedoor/{id}")]
        [ResponseType(typeof(DownlinkDataFormat))]
        public IHttpActionResult GetOpenTheDoor(int id)
        {
            string deviceId = id.ToString();

            DoorBooleanFormat data = new DoorBooleanFormat(true);
            string openDoorDataJson = JsonConvert.SerializeObject(data);
            String datatext = "0102AABB";

            DownlinkDataFormat downlinkData = new DownlinkDataFormat("tx", deviceId, 1, true, datatext, openDoorDataJson);
            String json2Send = JsonConvert.SerializeObject(downlinkData);

            foreach (var simulator in webSocketClient.getAllDevice())
            {
                if (simulator.getSaunaId() == id)
                {
                    webSocketClient.sendDownLink(simulator, json2Send);
                }
            }

            //return success
            return Ok(downlinkData);
        }

        // GET  api/hardwarecontroller/getCloseTheDoor/1
        [Route("api/hardwarecontroller/getCloseTheDoor/{id}")]
        [ResponseType(typeof(DownlinkDataFormat))]
        public IHttpActionResult GetCloseTheDoor(int id)
        {
            string deviceId = id.ToString();

            DoorBooleanFormat data = new DoorBooleanFormat(false);
            string openDoorDataJson = JsonConvert.SerializeObject(data);

            DownlinkDataFormat downlinkData = new DownlinkDataFormat("tx", deviceId, 1, true, "0102AABB", openDoorDataJson);
            string json2Send = JsonConvert.SerializeObject(downlinkData);

            foreach (var simulator in webSocketClient.getAllDevice())
            {
                if (simulator.getSaunaId() == id)
                {
                    webSocketClient.sendDownLink(simulator, json2Send);
                }
            }

            //return success
            return Ok(downlinkData);
        }


    }
}
