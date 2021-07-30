using sep4.IoTSimulator;
using sep4.IoTSimulator.Models;
using sep4.IoTSimulator.Hardware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sep4.IoTSimulator.WebSocket
{
    public class WebSocketThread
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();
        private WebSocketClient webSocketClient;

        /**
        * Constructor used for setting up and joining the web socket connection to Loriot
        * @param url - the URL for the socket connection
        */
        public WebSocketThread()
        {
            webSocketClient = WebSocketClient.getInstance();
        }

        public void run()
        {
            Timer timer = new Timer(delegate
            {
                foreach (IoTSimulator simulator in webSocketClient.getAllDevice())
                {
                    string dataJson = webSocketClient.receiveUplink(simulator);

                    UplinkDataFormat uplinkData = JsonSerializer.Deserialize<UplinkDataFormat>(dataJson);
                    string data = uplinkData.getData();
                    DataPoint dataPoint = JsonSerializer.Deserialize<DataPoint>(data);

                    //insert the data here
                    db.Datapoint.Add(datapoint);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        if (DatapointExists(datapoint.DatapointID))
                        {
                            return Conflict();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            },
            null,
            0,
            60000
            );

        }



    }


}