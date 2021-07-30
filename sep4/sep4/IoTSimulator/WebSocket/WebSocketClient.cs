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
        private List<IoTDeviceSimulator> simulators = new List<IoTDeviceSimulator>();

        /**
        * Constructor used for setting up and joining the web socket connection to Loriot
        * @param url - the URL for the socket connection
        */
        public WebSocketClient()
        {
            //make connection here
            //create and add some dummy simulators here
            addDevice(1);
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
        public bool addDevice(int saunaId)
        {
            lock (deviceListLock)
            {
                if (simulators.Count() > 0)
                {
                    foreach (var simulator in simulators)
                    {
                        if (simulator.getSaunaId() == saunaId)
                        {
                            return false;
                        }
                    }
                }
                simulators.Add(new IoTDeviceSimulator(saunaId));
                return true;
            }
        }

        public bool deleteDevice(int saunaId)
        {
            lock (deviceListLock)
            {
                if (simulators.Count() > 0)
                {
                    foreach (var simulator in simulators)
                    {
                        if (simulator.getSaunaId() == saunaId)
                        {
                            simulators.Remove(simulator);
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        public List<IoTDeviceSimulator> getAllDevice()
        {
            lock (deviceListLock)
            {
                return simulators;
            }
        }

        // Simulate the method sending down-link message to device
        // Must be in Json format according to https://github.com/ihavn/IoT_Semester_project/blob/master/LORA_NETWORK_SERVER.md
        public void sendDownLink(IoTDeviceSimulator simulator, String jsonTelegram)
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
