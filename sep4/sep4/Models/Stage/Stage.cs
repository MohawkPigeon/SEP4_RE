using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models.Stage
{
    public class Stage
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();
        
        public void insertIntoStage()
        {
            DateTime dateTimeNow = new DateTime();
            dateTimeNow = DateTime.Now;
            int count = 0;
            foreach (var item in db.Datapoint)
            {
                count++;
                StageDatapoint stage = new StageDatapoint();
                stage.CO2 = Int32.Parse(item.Co2.Trim());
                stage.DateTime = item.DateTime;
                stage.Humidity = Int32.Parse(item.Humidity.Trim());
                stage.Id = count;
                stage.ServoSettingAtTime = item.ServoSettingAtTime;
                stage.Temperature = Int32.Parse(item.Temperature.Trim());
                db.StageDatapoint.Add(stage);
            }
            count = 0;
            foreach (var item in db.Datapoint)
            {
                count++;
                StageDateDim stage = new StageDateDim();
                stage.DateTime = item.DateTime;
                stage.Date = item.DateTime.Value.Day;
                stage.Hour = item.DateTime.Value.Hour;
                stage.Id = count;
                stage.Minute = item.DateTime.Value.Minute;
                stage.Month = item.DateTime.Value.Month;
                stage.Second = item.DateTime.Value.Second;
                stage.Year = item.DateTime.Value.Year;
                db.StageDateDim.Add(stage);
            }
            count = 0;
            foreach (var item in db.Establishment)
            {       
                StageEstablishmentDIM stage = new StageEstablishmentDIM();
                stage.EstablishmentID = item.EstablishmentID;
                stage.LoadDate = dateTimeNow;
                stage.Name = item.Name;

                foreach (var sauna in item.Sauna)
                {
                    foreach (var reservation in sauna.Reservation)
                    {
                        if (reservation.User.Rights.Trim().Equals("Owner"))
                        {
                            count++;
                            stage.Id = count;
                            stage.Managerusername = reservation.User.Username;
                            stage.Rights = reservation.User.Rights;
                            db.StageEstablishmentDIM.Add(stage);
                        }
                    }
                }
            }
            count = 0;
            foreach (var item in db.Reservation)
            {
                count++;
                StageReservationDim stage = new StageReservationDim();
                stage.FromDateTime = item.FromDateTime;
                stage.Id = count;
                stage.LoadDate = dateTimeNow;
                stage.SaunaID = item.SaunaID.ToString();
                stage.ToDateTime = item.ToDateTime;
                stage.UserID = item.UserID;

                db.StageReservationDim.Add(stage);
            }
            count = 0;
            foreach (var item in db.Sauna)
            {
                count++;
                StageSaunaDim stage = new StageSaunaDim();
                stage.CO2Threshold = item.CO2Threshold;
                stage.EstablishmentID = item.EstablishmentID;
                stage.HumidityThreshold = item.HumidityThreshold;
                stage.Id = count;
                stage.LoadDate = dateTimeNow;
                stage.NameOrNumber = item.SaunaID.ToString();
                stage.SaunaID = item.SaunaID;
                stage.TemperatureThreshold = item.TemperatureThreshold;

                db.StageSaunaDim.Add(stage);
            }
            count = 0;
            foreach (var item in db.User)
            {
                if (item.Rights.Trim().Equals("Supervisor"))
                {               
                    StageSupervisorDim stage = new StageSupervisorDim();
                    stage.Username = item.Username;
                    stage.UserID = item.UserID;
                    stage.Rights = item.Rights;
                    stage.LoadDate = dateTimeNow;
                foreach (var history in item.NotificationHistory)
                    {
                        count++;
                        stage.EstShiftFromDate = history.DateTime.Value;
                        stage.EstShiftToDate = history.DateTime.Value;
                        stage.Id = count;

                        db.StageSupervisorDim.Add(stage);
                    }
                }
            }
            count = 0;
            foreach (var item in db.User)
            {
                if (item.Rights.Trim().Equals("User"))
                {
                    StageUserDim stage = new StageUserDim();
                    stage.ActiveSince = item.Reservation.First().FromDateTime;
                    stage.Id = count;
                    stage.LoadDate = dateTimeNow;
                    stage.Rights = item.Rights;
                    stage.UserID = item.UserID;
                    stage.Username = item.Username;

                    db.StageUserDim.Add(stage);
                }
            }
            db.SaveChanges();
        }
    }
}