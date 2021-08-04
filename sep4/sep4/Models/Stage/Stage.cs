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

            foreach (var item in db.Datapoint.Where(dp => dp.DateTime > DateTime.Now.AddDays(-1)))
            {
                    StageDatapoint stage = new StageDatapoint();
                    stage.DateTime = item.DateTime;
                    stage.SaunaID = item.SaunaID;
                    stage.Temperature = (int)Convert.ToDouble(item.Temperature);
                    stage.CO2 = (int)Convert.ToDouble(item.Co2);
                    stage.Humidity = (int)Double.Parse(item.Humidity);
                    stage.ServoSettingAtTime = item.ServoSettingAtTime;
                    db.StageDatapoint.Add(stage);           
            }

            DateTime lastInsertTime = db.StageDateDim.FirstOrDefault().DateTime.Value;
            while (dateTimeNow.CompareTo(lastInsertTime) > 0)
            {
                StageDateDim stage = new StageDateDim();
                stage.DateTime = lastInsertTime;
                stage.Date = lastInsertTime.Day;
                stage.Hour = lastInsertTime.Hour;
                stage.Minute = lastInsertTime.Minute;
                stage.Month = lastInsertTime.Month;
                stage.Second = lastInsertTime.Second;
                stage.Year = lastInsertTime.Year;
                db.StageDateDim.Add(stage);
                lastInsertTime.AddSeconds(1);
            }

            foreach (var item in db.Establishment.Where(x => x.DateTime > DateTime.Now.AddDays(-1)))
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
                            stage.Managerusername = reservation.User.Username;
                            stage.Rights = reservation.User.Rights;
                            db.StageEstablishmentDIM.Add(stage);
                        }
                    }
                }
            }

            foreach (var item in db.Reservation.Where(x => x.DateTime > DateTime.Now.AddDays(-1)))
            {
                StageReservationDim stage = new StageReservationDim();
                stage.FromDateTime = item.FromDateTime;
                stage.LoadDate = dateTimeNow;
                stage.SaunaID = item.SaunaID;
                stage.ToDateTime = item.ToDateTime;
                stage.UserID = item.UserID;

                db.StageReservationDim.Add(stage);
            }

            foreach (var item in db.Sauna.Where(x => x.DateTime > DateTime.Now.AddDays(-1)))
            {
                StageSaunaDim stage = new StageSaunaDim();
                stage.CO2Threshold = item.CO2Threshold;
                stage.EstablishmentID = item.EstablishmentID;
                stage.HumidityThreshold = item.HumidityThreshold;
                stage.LoadDate = dateTimeNow;
                stage.NameOrNumber = item.SaunaID.ToString();
                stage.SaunaID = item.SaunaID;
                stage.TemperatureThreshold = item.TemperatureThreshold;

                db.StageSaunaDim.Add(stage);
            }

            foreach (var item in db.User.Where(x => x.DateTime > DateTime.Now.AddDays(-1)))
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

                            stage.EstShiftFromDate = shiftStart.DateTime.Value;
                            stage.EstShiftToDate = history.DateTime.Value;

                            newShift = true;

                            db.StageSupervisorDim.Add(stage);
                        }
                        lastHistory = history;

                    }
                }
            }

            foreach (var item in db.User.Where(x => x.DateTime > DateTime.Now.AddDays(-1)))
            {
                if(item.Rights != null)
                if (item.Rights.Trim().Equals("User"))
                {
                    StageUserDim stage = new StageUserDim();
                    stage.ActiveSince = item.Reservation.First().FromDateTime;
                    stage.LoadDate = dateTimeNow;
                    stage.Rights = item.Rights;
                    stage.UserID = item.UserID;
                    stage.Username = item.Username;

                    db.StageUserDim.Add(stage);
                }
            }
            db.SaveChanges();
        }

        public void Transform()
        {
            foreach (var item in db.StageDateDim)
            {
                if (!item.DateTime.HasValue) item.DateTime = new DateTime(0, 0, 0, 0, 0, 0, 0);
                if (!item.Year.HasValue) item.Year = 0;
                if (!item.Month.HasValue) item.Month = 0;
                if (!item.Date.HasValue) item.Date = 0;
                if (!item.Hour.HasValue) item.Hour = 0;
                if (!item.Minute.HasValue) item.Minute = 0;
                if (!item.Second.HasValue) item.Second = 0;
            }

            foreach (var item in db.StageEstablishmentDIM)
            {
                if (!item.EstablishmentID.HasValue) item.EstablishmentID = -99;
                if (item.Name.Length==0) item.Name = "null";
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageReservationDim)
            {
                if (!item.SaunaID.HasValue) item.SaunaID = -99;
                if (!item.UserID.HasValue) item.UserID = -99;
                if (!item.FromDateTime.HasValue) item.FromDateTime = new DateTime(0, 0, 0, 0, 0, 0, 0);
                if (!item.ToDateTime.HasValue) item.ToDateTime = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageSaunaDim)
            {
                if (!item.SaunaID.HasValue) item.SaunaID = -99;
                if (!item.EstablishmentID.HasValue) item.EstablishmentID = -99;
                if (item.NameOrNumber.Length == 0) item.NameOrNumber = "null";
                if (item.TemperatureThreshold.Length == 0) item.TemperatureThreshold = "null";
                if (item.CO2Threshold.Length == 0) item.CO2Threshold = "null";
                if (item.HumidityThreshold.Length == 0) item.HumidityThreshold = "null";
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 

            }
            foreach (var item in db.StageSupervisorDim)
            {
                if (item.Username.Length == 0) item.Username = "null";
                if (item.Rights.Length == 0) item.Rights = "null";
                if (!item.EstShiftFromDate.HasValue) item.EstShiftFromDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.EstShiftToDate.HasValue) item.EstShiftToDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageUserDim)
            {
                if (item.Username.Length == 0) item.Username = "null";
                if (item.Rights.Length == 0) item.Rights = "null";
                if (!item.ActiveSince.HasValue) item.ActiveSince = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
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
                establishmentDim.ValidTo = item.ValidTo;
                db.EstablishmentDim.Add(establishmentDim);
            }
            foreach (var item in db.StageReservationDim) {
                ReservationDim ReservationDim = new ReservationDim();
                ReservationDim.SaunaID = (int)item.SaunaID;
                ReservationDim.UserID = item.UserID.Value;
                ReservationDim.FromDateTime = item.FromDateTime.Value;
                ReservationDim.ToDateTime = item.FromDateTime.Value;
                ReservationDim.LoadDate = item.LoadDate.Value;
                ReservationDim.ValidTo = item.ValidTo;
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
                saunaDim.ValidTo = item.ValidTo;
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
                supervisorDim.ValidTo = item.ValidTo;
                db.SupervisorDim.Add(supervisorDim);
            }
            foreach (var item in db.StageUserDim) {
                UserDim stageUser = new UserDim();
                stageUser.UserID = item.UserID.Value;
                stageUser.Username = item.Username;
                stageUser.Rights = item.Rights;
                stageUser.ActiveSince = item.ActiveSince.Value;
                stageUser.LoadDate = item.LoadDate.Value;
                stageUser.ValidTo = item.ValidTo;
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