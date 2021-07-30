using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sep4.Models.Stage
{
    public class Stage
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();
        DateTime dateTimeNow = new DateTime();
        public void insertIntoStage()
        {
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
                    NotificationHistory lastHistory = new NotificationHistory();
                    NotificationHistory shiftStart = new NotificationHistory();
                    Boolean newShift = false;
                    foreach (var history in item.NotificationHistory)
                    {
                        if (newShift == true)
                        {
                            shiftStart.DateTime = history.DateTime.Value;
                            newShift = false;
                        }
                        if (!shiftStart.DateTime.HasValue) {
                            shiftStart.DateTime = history.DateTime.Value;
                        }
                        if (!lastHistory.DateTime.HasValue)
                        {
                            lastHistory.DateTime = history.DateTime.Value;
                        }
                        if (lastHistory.DateTime.Value.AddHours(1)<history.DateTime.Value)
                        {
                            StageSupervisorDim stage = new StageSupervisorDim();
                            stage.Username = item.Username;
                            stage.UserID = item.UserID;
                            stage.Rights = item.Rights;
                            stage.LoadDate = dateTimeNow;

                            count++;
                            stage.EstShiftFromDate = shiftStart.DateTime.Value;
                            stage.EstShiftToDate = history.DateTime.Value;
                            stage.Id = count;

                            newShift = true;

                            db.StageSupervisorDim.Add(stage);
                        }
                        lastHistory = history;

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
        public void Load() { 
            
            foreach (var item in db.StageDateDim) {
                DateDim dateDim = new DateDim();
                dateDim.DateTime =  item.DateTime.Value;
                dateDim.Year = item.Year.Value;
                dateDim.Month = item.Month.Value;
                dateDim.Date = item.Date.Value;
                dateDim.Hour = item.Hour.Value;
                dateDim.Minute = item.Minute.Value;
                dateDim.Second = item.Second.Value;
                db.DateDim.Add(dateDim);
                db.SaveChanges();
            }
            foreach (var item in db.StageEstablishmentDIM) {
                EstablishmentDim establishmentDim = new EstablishmentDim();
                establishmentDim.EstablishmentID = item.EstablishmentID.Value;
                establishmentDim.Name = item.Name;
                establishmentDim.LoadDate = item.LoadDate.Value;
                db.EstablishmentDim.Add(establishmentDim);
                db.SaveChanges();
            }
            foreach (var item in db.StageReservationDim) {
                ReservationDim ReservationDim = new ReservationDim();
                ReservationDim.SaunaID = item.SaunaID;
                ReservationDim.UserID = item.UserID.Value;
                ReservationDim.FromDateTime = item.FromDateTime.Value;
                ReservationDim.ToDateTime = item.FromDateTime.Value;
                ReservationDim.LoadDate = item.LoadDate.Value;
                db.ReservationDim.Add(ReservationDim);
                db.SaveChanges();
            }
            foreach (var item in db.StageSaunaDim) {
                SaunaDim saunaDim = new SaunaDim();
                saunaDim.SaunaID = item.SaunaID.Value;
                saunaDim.EstablishmentID = item.EstablishmentID.Value;
                saunaDim.NameOrNumber = item.NameOrNumber;
                saunaDim.TemperatureThreshold = item.TemperatureThreshold;
                saunaDim.CO2Threshold = item.CO2Threshold;
                saunaDim.HumidityThreshold = item.HumidityThreshold;
                saunaDim.LoadDate = item.LoadDate.Value;
                db.SaunaDim.Add(saunaDim);
                db.SaveChanges();
            }
            foreach (var item in db.StageSupervisorDim) {
                SupervisorDim supervisorDim = new SupervisorDim();
                supervisorDim.UserID = item.UserID;
                supervisorDim.Username = item.Username;
                supervisorDim.Rights = item.Rights;
                supervisorDim.EstShiftFromDate = item.EstShiftFromDate.Value;
                supervisorDim.EstShiftToDate = item.EstShiftToDate.Value;
                supervisorDim.LoadDate = item.LoadDate.Value;
            }
            foreach (var item in db.StageUserDim) {
                StageUserDim stageUser = new StageUserDim();
                stageUser.UserID = item.UserID;
                stageUser.Username = item.Username;
                stageUser.Rights = item.Rights;
                stageUser.ActiveSince = item.ActiveSince;
                stageUser.LoadDate = stageUser.LoadDate;
            }
            foreach (var item in db.StageDatapoint) { }
        } 
    }
}