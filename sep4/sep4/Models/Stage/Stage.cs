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
        int daysSinceLastUpdate = 0;

        public Stage(int daysSinceLastUpdate)
        {
            this.daysSinceLastUpdate = daysSinceLastUpdate;
        }


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
        public void RemoveDim()
        {
            db.DateDim.RemoveRange(db.DateDim);
            db.EstablishmentDim.RemoveRange(db.EstablishmentDim);
            db.ReservationDim.RemoveRange(db.ReservationDim);
            db.SaunaDim.RemoveRange(db.SaunaDim);
            db.SupervisorDim.RemoveRange(db.SupervisorDim);
            db.UserDim.RemoveRange(db.UserDim);
            db.SaveChanges();
        }

        public void InsertIntoStage()
        {
            dateTimeNow = DateTime.Now;
            DateTime validToDate = DateTime.Now.AddYears(100);
            DateTime daysSinceLastUpdateDate = DateTime.Now.AddDays(-daysSinceLastUpdate);

            foreach (var item in db.Datapoint.Where(dp => dp.DateTime > daysSinceLastUpdateDate))
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

            StageDateDim stageDateDim = db.StageDateDim.FirstOrDefault();
            if (stageDateDim != null)
            {
                DateTime lastInsertTime = stageDateDim.DateTime.Value;
                while (dateTimeNow > lastInsertTime)
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
                    lastInsertTime = lastInsertTime.AddMinutes(1);
                }
            }
            else
            {
                var small = db.Datapoint.Where(dp => dp.DateTime > daysSinceLastUpdateDate).ToList();
                int smallId = small.First().DatapointID;
                foreach (var item in small)
                {
                    if(item.DatapointID < smallId)
                    {
                        smallId = item.DatapointID;
                    }           
                }
                DateTime lastInsertTime = db.Datapoint.Where(dp => dp.DatapointID == smallId).First().DateTime;
                while (dateTimeNow > lastInsertTime)
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
                    lastInsertTime = lastInsertTime.AddMinutes(1);
                }
            }

            foreach (var item in db.Establishment.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {       
                StageEstablishmentDIM stage = new StageEstablishmentDIM();
                stage.EstablishmentID = item.EstablishmentID;
                stage.LoadDate = dateTimeNow;
                stage.Name = item.Name;
                stage.ValidTo = validToDate;

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

            foreach (var item in db.Reservation.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {
                StageReservationDim stage = new StageReservationDim();
                stage.FromDateTime = item.FromDateTime;
                stage.LoadDate = dateTimeNow;
                stage.SaunaID = item.SaunaID;
                stage.ToDateTime = item.ToDateTime;
                stage.UserID = item.UserID;
                stage.ValidTo = validToDate;

                db.StageReservationDim.Add(stage);
            }

            foreach (var item in db.Sauna.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {
                StageSaunaDim stage = new StageSaunaDim();
                stage.CO2Threshold = item.CO2Threshold;
                stage.EstablishmentID = item.EstablishmentID;
                stage.HumidityThreshold = item.HumidityThreshold;
                stage.LoadDate = dateTimeNow;
                stage.NameOrNumber = item.SaunaID.ToString();
                stage.SaunaID = item.SaunaID;
                stage.TemperatureThreshold = item.TemperatureThreshold;
                stage.ValidTo = validToDate;

                db.StageSaunaDim.Add(stage);
            }

            foreach (var item in db.User.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {
                if(item.Rights != null)
                if (item.Rights.Trim().Equals("Supervisor"))
                {
                    foreach (var reservation in item.Reservation)
                    {
                            StageSupervisorDim stage = new StageSupervisorDim();
                            stage.Username = item.Username;
                            stage.UserID = item.UserID;
                            stage.Rights = item.Rights;
                            stage.LoadDate = dateTimeNow;
                            stage.ValidTo = validToDate;

                            stage.EstShiftFromDate = reservation.FromDateTime;
                            stage.EstShiftToDate = reservation.ToDateTime;

                            db.StageSupervisorDim.Add(stage);
                    }
                }
            }

            foreach (var item in db.User.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {
                if(item.Rights != null)
                if (item.Rights.Trim().Equals("User"))
                {
                    StageUserDim stage = new StageUserDim();
                        if(item.Reservation.Count > 0)
                        if(item.Reservation.LastOrDefault() != null)
                        {
                            stage.ActiveSince = item.Reservation.LastOrDefault().FromDateTime;
                        }
                    stage.LoadDate = dateTimeNow;
                    stage.Rights = item.Rights;
                    stage.UserID = item.UserID;
                    stage.Username = item.Username;
                    stage.ValidTo = validToDate;

                    db.StageUserDim.Add(stage);
                }
            }
            db.SaveChanges();
        }

        public void Transform()
        {
            DateTime daysSinceLastUpdateDate = DateTime.Now.AddDays(-daysSinceLastUpdate);
            foreach (var item in db.StageDateDim.Where(x => x.DateTime > daysSinceLastUpdateDate))
            {
                if (!item.DateTime.HasValue) item.DateTime = new DateTime(0, 0, 0, 0, 0, 0, 0);
                if (!item.Year.HasValue) item.Year = 0;
                if (!item.Month.HasValue) item.Month = 0;
                if (!item.Date.HasValue) item.Date = 0;
                if (!item.Hour.HasValue) item.Hour = 0;
                if (!item.Minute.HasValue) item.Minute = 0;
                if (!item.Second.HasValue) item.Second = 0;
            }

            foreach (var item in db.StageEstablishmentDIM.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow))
            {
                if (!item.EstablishmentID.HasValue) item.EstablishmentID = -99;
                if (item.Name.Length==0) item.Name = "null";
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageReservationDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow))
            {
                if (!item.SaunaID.HasValue) item.SaunaID = -99;
                if (!item.UserID.HasValue) item.UserID = -99;
                if (!item.FromDateTime.HasValue) item.FromDateTime = new DateTime(0, 0, 0, 0, 0, 0, 0);
                if (!item.ToDateTime.HasValue) item.ToDateTime = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageSaunaDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow))
            {
                if (!item.SaunaID.HasValue) item.SaunaID = -99;
                if (!item.EstablishmentID.HasValue) item.EstablishmentID = -99;
                if (item.NameOrNumber.Length == 0) item.NameOrNumber = "null";
                if (item.TemperatureThreshold.Length == 0) item.TemperatureThreshold = "null";
                if (item.CO2Threshold.Length == 0) item.CO2Threshold = "null";
                if (item.HumidityThreshold.Length == 0) item.HumidityThreshold = "null";
                //if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 

            }
            foreach (var item in db.StageSupervisorDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow))
            {
                if (item.Username.Length == 0) item.Username = "null";
                if (item.Rights.Length == 0) item.Rights = "null";
                if (!item.EstShiftFromDate.HasValue) item.EstShiftFromDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.EstShiftToDate.HasValue) item.EstShiftToDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
                if (!item.LoadDate.HasValue) item.LoadDate = new DateTime(0, 0, 0, 0, 0, 0, 0); 
            }
            foreach (var item in db.StageUserDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow))
            {
                if (item.Username.Length == 0) item.Username = "null";
                if (item.Rights.Length == 0) item.Rights = "null";
                if (!item.ActiveSince.HasValue) item.ActiveSince = DateTime.Now.AddYears(100);
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
            DateTime daysSinceLastUpdateDate = DateTime.Now.AddDays(-daysSinceLastUpdate);

            foreach (var item in db.StageDateDim.Where(x => x.DateTime > daysSinceLastUpdateDate)) {
                DateDim dateDim = new DateDim();
                dateDim.DateDimID = item.Id;
                dateDim.DateTime =  item.DateTime.Value;
                dateDim.Year = item.Year.Value;
                dateDim.Month = item.Month.Value;
                dateDim.Date = item.Date.Value;
                dateDim.Hour = item.Hour.Value;
                dateDim.Minute = item.Minute.Value;
                dateDim.Second = item.Second.Value;
                db.DateDim.Add(dateDim);
            }
            foreach (var item in db.StageEstablishmentDIM.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow)) {
                EstablishmentDim establishmentDim = new EstablishmentDim();
                establishmentDim.EstablishmentDimID = item.Id;
                establishmentDim.EstablishmentID = item.EstablishmentID.Value;
                establishmentDim.Name = item.Name;
                establishmentDim.LoadDate = item.LoadDate.Value;
                establishmentDim.ValidTo = item.ValidTo;
                establishmentDim.ManagerUsername = item.Managerusername;
                establishmentDim.Rights = item.Rights;
                db.EstablishmentDim.Add(establishmentDim);
            }
            foreach (var item in db.StageReservationDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow)) {
                ReservationDim ReservationDim = new ReservationDim();
                ReservationDim.ReservationDimID = item.Id;
                ReservationDim.SaunaID = (int)item.SaunaID;
                ReservationDim.UserID = item.UserID.Value;
                ReservationDim.FromDateTime = item.FromDateTime.Value;
                ReservationDim.ToDateTime = item.FromDateTime.Value;
                ReservationDim.LoadDate = item.LoadDate.Value;
                ReservationDim.ValidTo = item.ValidTo;
                db.ReservationDim.Add(ReservationDim);
            }
            foreach (var item in db.StageSaunaDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow)) {
                SaunaDim saunaDim = new SaunaDim();
                saunaDim.SaunaDimID = item.Id;
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
            foreach (var item in db.StageSupervisorDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow)) {
                SupervisorDim supervisorDim = new SupervisorDim();
                supervisorDim.SupervisorDimID = item.Id;
                supervisorDim.UserID = item.UserID;
                supervisorDim.Username = item.Username;
                supervisorDim.Rights = item.Rights;
                supervisorDim.EstShiftFromDate = item.EstShiftFromDate.Value;
                supervisorDim.EstShiftToDate = item.EstShiftToDate.Value;
                supervisorDim.LoadDate = item.LoadDate.Value;
                supervisorDim.ValidTo = item.ValidTo;
                db.SupervisorDim.Add(supervisorDim);
            }
            foreach (var item in db.StageUserDim.Where(x => x.LoadDate > daysSinceLastUpdateDate && x.ValidTo > dateTimeNow)) {
                UserDim stageUser = new UserDim();
                stageUser.UserDimID = item.Id;
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
            DateTime daysSinceLastUpdateDate = DateTime.Now.AddDays(-daysSinceLastUpdate);
            foreach (var item in db.StageDatapoint)
            {
                SaunaFact saunaFact = new SaunaFact();
                saunaFact.DateDimID = db.DateDim.Where(d => d.DateTime.Year == item.DateTime.Value.Year && d.DateTime.Month == item.DateTime.Value.Month && d.DateTime.Day == item.DateTime.Value.Day && d.DateTime.Hour == item.DateTime.Value.Hour && d.DateTime.Minute == item.DateTime.Value.Minute).FirstOrDefault().DateDimID;
                saunaFact.SaunaDimID = db.SaunaDim.Where(s => s.SaunaID == item.SaunaID).FirstOrDefault().SaunaDimID;
                saunaFact.ReservationDimID = db.ReservationDim.Where(re => re.SaunaID == item.SaunaID && re.FromDateTime < item.DateTime && item.DateTime > re.ToDateTime).FirstOrDefault().ReservationDimID;
                saunaFact.SupervisorDimID = db.SupervisorDim.Where(su => su.EstShiftFromDate < item.DateTime && item.DateTime < su.EstShiftToDate).FirstOrDefault().SupervisorDimID;
                saunaFact.EstablishmentDimID = db.EstablishmentDim.Where(x => x.EstablishmentID == db.SaunaDim.Where(y => y.SaunaID == item.SaunaID).FirstOrDefault().EstablishmentID).FirstOrDefault().EstablishmentDimID;
                saunaFact.UserDimID = db.UserDim.Where(u => u.UserID == db.ReservationDim.Where(x => x.SaunaID == item.SaunaID).FirstOrDefault().UserID).FirstOrDefault().UserDimID;
                saunaFact.CO2 = (int)item.CO2;
                saunaFact.Temperature = (int)item.Temperature;
                saunaFact.Humidity = (int)item.Humidity;
                saunaFact.ServoSettingAtTime = item.ServoSettingAtTime;
                db.SaunaFact.Add(saunaFact);
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
    }
}