using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class UplinkDataFormat
    {
        public string cmd { get; set; }
        public string EUI { get; set; }
        public long ts { get; set; }
        public bool ack { get; set; }
        public long fcnt { get; set; }
        public long port { get; set; }
        public string data { get; set; }

        public UplinkDataFormat(string cmd, string eUI, long ts, bool ack, long fcnt, long port, string data)
        {
            this.cmd = cmd;
            EUI = eUI;
            this.ts = ts;
            this.ack = ack;
            this.fcnt = fcnt;
            this.port = port;
            this.data = data;
        }
    }
}
