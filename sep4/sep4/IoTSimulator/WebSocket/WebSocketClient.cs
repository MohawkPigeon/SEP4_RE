using sep4.IoTSimulator.Hardware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.WebSocket
{
    public class WebSocketClient
    {
        private static WebSocketClient instance;
        private List<IoTDeviceSimulator> simulators;

        /**
        * Constructor used for setting up and joining the web socket connection to Loriot
        * @param url - the URL for the socket connection
        */
        private WebSocketClient()
        {
            //make connection here
            //create and add some dummy simulators here
            addDevice(new IoTDeviceSimulator(1,1));
            addDevice(new IoTDeviceSimulator(1,2));
            addDevice(new IoTDeviceSimulator(2,1));
            addDevice(new IoTDeviceSimulator(3,1));
            addDevice(new IoTDeviceSimulator(4,1));
            addDevice(new IoTDeviceSimulator(5,1));
            addDevice(new IoTDeviceSimulator(6,1));
            addDevice(new IoTDeviceSimulator(7,1));
            addDevice(new IoTDeviceSimulator(8,1));
            addDevice(new IoTDeviceSimulator(9,1));
        }

        private static readonly object instanceLock = new object ();  
        public static WebSocketClient getInstance()
        {
            lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new WebSocketClient();
                    }
                    return instance;
                }
        }

        private static readonly object deviceListLock = new object();
        public bool addDevice(int saunaId,int datapointId)
        {
            lock (deviceListLock)
            {
                foreach (IoTDeviceSimulator simulator in simulators)
                {
                    if (simulator.getSaunaId() == saunaId)
                    {
                        return false;
                    }
                }
                simulators.Add(new IoTDeviceSimulator(saunaId, datapointId));
                return true;
            }
        }

        public bool delateDevice(int saunaId, int datapointId)
        {
            lock (deviceListLock)
            {
                foreach (IoTDeviceSimulator simulator in simulators)
                {
                    if (simulator.getSaunaId() == saunaId&& simulator.getDatapointId() == datapointId)
                    {
                        simulators.Remove(simulator);
                        return true;
                    }
                }
                return false;
            }
        }


        public IoTDeviceSimulator getAllDevice()
        {
            lock (deviceListLock)
            {
                return simulators;
            }
        }

        // Simulate the method sending down-link message to device
        // Must be in Json format according to https://github.com/ihavn/IoT_Semester_project/blob/master/LORA_NETWORK_SERVER.md
        public string sendDownLink(IoTDeviceSimulator simulator, String jsonTelegram)
        {
            simulator.controlTheDoor(jsonTelegram);
        }


        /**
         * Simulator the method called when new information is heard in the Loriot network server
         * @return a completion stage action i.e printing out a message
         */
        public string receiveUplink(IoTDeviceSimulator simulator)
        {
            return simulator.uplinkDataJson(); 
        }


    }

 
}
