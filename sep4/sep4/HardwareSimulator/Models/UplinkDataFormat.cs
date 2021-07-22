using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.HardwareSimulator.Models
{
    public class UplinkDataFormat
    {
        private string cmd;
        private string EUI;
        private long ts;
        private bool ack;
        private long fcnt;
        private long port;
        private string data;

        public string getData()
        {
            return data;
        }

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
