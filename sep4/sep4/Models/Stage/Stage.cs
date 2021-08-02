using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace sep4.Models.Stage
{
    public class Stage
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();
        DateTime dateTimeNow = new DateTime();

        public void RemoveStage()
        {
            db.StageDatapoint.RemoveRange(db.StageDatapoint);
            db.StageDateDim.RemoveRange(db.StageDateDim);
            db.StageEstablishmentDIM.RemoveRange(db.StageEstablishmentDIM);
            db.StageReservationDim.RemoveRange(db.StageReservationDim);
            db.StageSaunaDim.RemoveRange(db.StageSaunaDim);
            db.StageSupervisorDim.RemoveRange(db.StageSupervisorDim);
            db.StageUserDim.RemoveRange(db.StageUserDim);
            db.SaveChanges();
        }

        public void InsertIntoStage()
        {
            dateTimeNow = DateTime.Now;
            int count = 0;
            foreach (var item in db.Datapoint)
            {
                //if(item.DateTime > dateTimeNow.AddDays(-1)){
                    count++;
                    StageDatapoint stage = new StageDatapoint();
                    stage.Id = count;
                    stage.DateTime = item.DateTime;
                    stage.SaunaID = item.SaunaID;
                    stage.Temperature = (int)Convert.ToDouble(item.Temperature);
                    stage.CO2 = (int)Convert.ToDouble(item.Co2);
                    stage.Humidity = (int)Double.Parse(item.Humidity);
                    stage.ServoSettingAtTime = item.ServoSettingAtTime;
                    db.StageDatapoint.Add(stage);
                //}            
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
                        if(reservation.User.Rights != null)
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
                stage.SaunaID = item.SaunaID;
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
                if(item.Rights != null)
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
                if(item.Rights != null)
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
            }
            foreach (var item in db.StageEstablishmentDIM) {
                EstablishmentDim establishmentDim = new EstablishmentDim();
                establishmentDim.EstablishmentID = item.EstablishmentID.Value;
                establishmentDim.Name = item.Name;
                establishmentDim.LoadDate = item.LoadDate.Value;
                db.EstablishmentDim.Add(establishmentDim);
            }
            foreach (var item in db.StageReservationDim) {
                ReservationDim ReservationDim = new ReservationDim();
                ReservationDim.SaunaID = (int)item.SaunaID;
                ReservationDim.UserID = item.UserID.Value;
                ReservationDim.FromDateTime = item.FromDateTime.Value;
                ReservationDim.ToDateTime = item.FromDateTime.Value;
                ReservationDim.LoadDate = item.LoadDate.Value;
                db.ReservationDim.Add(ReservationDim);
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
            }
            foreach (var item in db.StageSupervisorDim) {
                SupervisorDim supervisorDim = new SupervisorDim();
                supervisorDim.UserID = item.UserID;
                supervisorDim.Username = item.Username;
                supervisorDim.Rights = item.Rights;
                supervisorDim.EstShiftFromDate = item.EstShiftFromDate.Value;
                supervisorDim.EstShiftToDate = item.EstShiftToDate.Value;
                supervisorDim.LoadDate = item.LoadDate.Value;
                db.SupervisorDim.Add(supervisorDim);
            }
            foreach (var item in db.StageUserDim) {
                UserDim stageUser = new UserDim();
                stageUser.UserID = item.UserID.Value;
                stageUser.Username = item.Username;
                stageUser.Rights = item.Rights;
                stageUser.ActiveSince = item.ActiveSince.Value;
                stageUser.LoadDate = item.LoadDate.Value;
                db.UserDim.Add(stageUser);
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
           
        } 

        public void LoadFact()
        {
            foreach (var item in db.StageDatapoint)
            {
                SaunaFact saunaFact = new SaunaFact();
                saunaFact.DateDimID = db.DateDim.Where(d => d.DateTime == item.DateTime.Value).FirstOrDefault().DateDimID;
                saunaFact.SaunaDimID = db.SaunaDim.Where(s => s.SaunaID == item.SaunaID).FirstOrDefault().SaunaDimID;
                saunaFact.ReservationDimID = db.ReservationDim.Where(re => re.SaunaID == item.SaunaID && re.FromDateTime < item.DateTime && item.DateTime > re.ToDateTime).FirstOrDefault().ReservationDimID;
                saunaFact.SupervisorDimID = db.SupervisorDim.Where(su => su.EstShiftFromDate < item.DateTime && item.DateTime < su.EstShiftToDate).FirstOrDefault().SupervisorDimID;
                saunaFact.EstablishmentDimID = db.EstablishmentDim.Where(e => e.EstablishmentID == saunaFact.SaunaDim.EstablishmentID).FirstOrDefault().EstablishmentDimID;
                saunaFact.UserDimID = db.UserDim.Where(u => u.UserID == saunaFact.ReservationDim.UserID).FirstOrDefault().UserDimID;
                db.SaunaFact.Add(saunaFact);
            }
            db.SaveChanges();
        }
    }
}