using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.Models
{
    public class DownlinkDataFormat
    {
        private string cmd;
        private string EUI;
        private long port;
        private bool confirmed;
        private string data;

        public DownlinkDataFormat(string cmd, string eUI, long port)
        {
            this.cmd = cmd;
            EUI = eUI;
            this.port = port;
        }

        public DownlinkDataFormat(string cmd, string eUI, long port, bool confirmed) : this(cmd, eUI, port)
        {
            this.confirmed = confirmed;
        }

        public DownlinkDataFormat(string cmd, string eUI, long port, string data) : this(cmd, eUI, port)
        {
            this.data = data;
        }

        public DownlinkDataFormat(string cmd, string eUI, long port, bool confirmed, string data) : this(cmd, eUI, port, confirmed)
        {
            this.data = data;
        }

        public string getData()
        {
            return data;
        }

    }
}
