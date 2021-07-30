using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DownlinkDataFormat
    {
        public string cmd { get; set; }
        public string EUI { get; set; }
        public long port { get; set; }
        public bool confirmed { get; set; }
        public string data { get; set; }
        public string openDoorDataJson { get; set; }
        [JsonConstructor]
        public DownlinkDataFormat(string cmd, string eUI, long port, bool confirmed, string data, string openDoorDataJson)
        {
            this.cmd = cmd;
            EUI = eUI;
            this.port = port;
            this.confirmed = confirmed;
            this.data = data;
            this.openDoorDataJson = openDoorDataJson;
        }
    }
}
