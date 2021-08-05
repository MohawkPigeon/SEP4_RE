using sep4.IoTSimulator;
using sep4.IoTSimulator.Models;
using sep4.IoTSimulator.Hardware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.Timers;

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

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(run);
            aTimer.Interval = 60000;
            aTimer.Enabled = true;
        }

        public void run(object source, ElapsedEventArgs e)
        {
            foreach (var simulator in webSocketClient.getAllDevice())
            {
                string dataJson = webSocketClient.receiveUplink(simulator);

                UplinkDataFormat uplinkData = JsonConvert.DeserializeObject<UplinkDataFormat>(dataJson);
                string data = uplinkData.data;
                if (data != null)
                {
                    DataPoint dataPoint = JsonConvert.DeserializeObject<DataPoint>(data);

                    //insert the data here
                    // converting between models
                    Datapoint datapoint = new Datapoint();
                    datapoint.Co2 = dataPoint.CO2.ToString();
                    datapoint.DateTime = dataPoint.Time;
                    datapoint.Humidity = dataPoint.Humidity.ToString();
                    datapoint.SaunaID = (int)dataPoint.SaunaId;
                    datapoint.ServoSettingAtTime = dataPoint.ServoSetting.ToString();
                    datapoint.Temperature = dataPoint.Temperature.ToString();
                    db.Datapoint.Add(datapoint);

                    Sauna sauna = db.Sauna.Find(datapoint.SaunaID);
                    if(double.Parse(datapoint.Co2.Trim()) > double.Parse(sauna.CO2Threshold.Trim()) || double.Parse(datapoint.Humidity.Trim()) > double.Parse(sauna.HumidityThreshold.Trim()) || double.Parse(datapoint.Temperature.Trim()) > double.Parse(sauna.TemperatureThreshold.Trim()))
                    {
                        NotificationHistory notificationHistory = new NotificationHistory();
                        notificationHistory.DateTime = DateTime.Now;
                        Reservation reservation = sauna.Reservation.Where(x => x.User.Rights.Trim() == "Supervisor").FirstOrDefault();
                        if(reservation != null)
                        {
                            notificationHistory.UserID = reservation.UserID;
                            notificationHistory.User = reservation.User;
                            db.NotificationHistory.Add(notificationHistory);
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        throw;
                    }
                }
            }

        }



    }


}