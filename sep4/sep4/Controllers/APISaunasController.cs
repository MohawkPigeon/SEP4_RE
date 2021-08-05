using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using sep4;
using sep4.Models;

namespace sep4.Controllers
{
    public class APISaunasController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APISaunas
        [ResponseType(typeof(List<SaunaDTO>))]
        public IHttpActionResult GetSaunas()
        {
            List<SaunaDTO> saunaDTOs = new List<SaunaDTO>();
            foreach (var sauna in db.Sauna.ToList())
            {
                SaunaDTO saunaDTO = new SaunaDTO(sauna.SaunaID,sauna.EstablishmentID, sauna.TemperatureThreshold, sauna.CO2Threshold, sauna.HumidityThreshold);
                saunaDTOs.Add(saunaDTO);
            }

            return Ok(saunaDTOs);
        }

        // GET: api/APISauna/5
        [Route("api/APISauna/{id}")]
        [ResponseType(typeof(SaunaDTO))]
        public IHttpActionResult GetSauna(int id)
        {
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return NotFound();
            }
            List<DatapointDTO> datapoints = new List<DatapointDTO>();
            foreach (var datapoint in sauna.Datapoint.ToList())
            {
                DatapointDTO datapointDTO = new DatapointDTO(datapoint.DatapointID, datapoint.SaunaID, datapoint.DateTime, datapoint.Temperature, datapoint.Co2, datapoint.Humidity, datapoint.ServoSettingAtTime);
                datapoints.Add(datapointDTO);
            }

            SaunaDTO saunaDTO = new SaunaDTO(sauna.SaunaID, sauna.EstablishmentID, sauna.TemperatureThreshold, sauna.CO2Threshold, sauna.HumidityThreshold, datapoints);
            return Ok(saunaDTO);
        }

        // PUT: api/APISaunas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSauna(int id, Sauna sauna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sauna.SaunaID)
            {
                return BadRequest();
            }

            db.Entry(sauna).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaunaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// PUT: api/APISaunas/{servoSetting}
        //[Route("api/APISaunas/{servoSetting}")]
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutSaunaSetting(ServoSetting servoSetting)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Sauna sauna = db.Sauna.Find(servoSetting.SaunaID);
        //    sauna.ServoSetting.Add(servoSetting);

        //    db.Entry(sauna).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SaunaExists(servoSetting.SaunaID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}
        // POST: api/APISaunas
        [ResponseType(typeof(Sauna))]
        public IHttpActionResult PostSauna(Sauna sauna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            sauna.DateTime = DateTime.Now;
            db.Sauna.Add(sauna);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SaunaExists(sauna.SaunaID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sauna.SaunaID }, sauna);
        }

        // DELETE: api/APISaunas/5
        [ResponseType(typeof(Sauna))]
        public IHttpActionResult DeleteSauna(int id)
        {
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return NotFound();
            }
            StageSaunaDim stageSauna = db.StageSaunaDim.Where(ss => ss.SaunaID == sauna.SaunaID && ss.ValidTo > DateTime.Now).FirstOrDefault();
            stageSauna.ValidTo = DateTime.Now.AddDays(-1);

            db.Sauna.Remove(sauna);
            db.SaveChanges();

            return Ok(sauna);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaunaExists(int id)
        {
            return db.Sauna.Count(e => e.SaunaID == id) > 0;
        }
    }
}