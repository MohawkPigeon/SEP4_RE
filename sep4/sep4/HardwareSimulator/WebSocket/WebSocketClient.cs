using sep4.HardwareSimulator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.HardwareSimulator.WebSocket
{
    public class WebSocketClient
    {
        private DataGenerator dataGenerator;

        /**
        * Constructor used for setting up and joining the web socket connection to Loriot
        * @param url - the URL for the socket connection
        */
        public WebSocketClient()
        {
            //make connection here
            dataGenerator = new DataGenerator();
        }


        // Simulate the method sending down-link message to device
        // Must be in Json format according to https://github.com/ihavn/IoT_Semester_project/blob/master/LORA_NETWORK_SERVER.md
        public string sendDownLink(String jsonTelegram)
        {
            return receiveUplink();
        }


        /**
         * Simulator the method called when new information is heard in the Loriot network server
         * @return a completion stage action i.e printing out a message
         */
        private string receiveUplink()
        {
            string dataJson = dataGenerator.DataJson();

            return dataJson;
        }


    }

 
}
