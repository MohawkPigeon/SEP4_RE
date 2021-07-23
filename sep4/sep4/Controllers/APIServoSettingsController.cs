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
    public class APIServoSettingsController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APIServoSettings
        [ResponseType(typeof(List<ServoSettingDTO>))]
        public IHttpActionResult GetServoSettings()
        {
            List<ServoSettingDTO> servoSettingDTOs = new List<ServoSettingDTO>();
            foreach (var servoSetting in db.ServoSetting.ToList())
            {
                ServoSettingDTO servoSettingDTO = new ServoSettingDTO(servoSetting.ServoSettingID, servoSetting.SaunaID, servoSetting.Datetime, servoSetting.Setting);
                servoSettingDTOs.Add(servoSettingDTO);
            }

            return Ok(servoSettingDTOs);
        }

        // GET: api/APIServoSettings/5
        [ResponseType(typeof(ServoSettingDTO))]
        public IHttpActionResult GetServoSetting(int id)
        {
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            if (servoSetting == null)
            {
                return NotFound();
            }
            ServoSettingDTO servoSettingDTO = new ServoSettingDTO(servoSetting.ServoSettingID, servoSetting.SaunaID, servoSetting.Datetime, servoSetting.Setting);

            return Ok(servoSettingDTO);
        }

        // PUT: api/APIServoSettings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServoSetting(int id, ServoSetting servoSetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servoSetting.ServoSettingID)
            {
                return BadRequest();
            }

            db.Entry(servoSetting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServoSettingExists(id))
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

        // POST: api/APIServoSettings
        [ResponseType(typeof(ServoSetting))]
        public IHttpActionResult PostServoSetting(ServoSetting servoSetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServoSetting.Add(servoSetting);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ServoSettingExists(servoSetting.ServoSettingID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = servoSetting.ServoSettingID }, servoSetting);
        }

        // DELETE: api/APIServoSettings/5
        [ResponseType(typeof(ServoSetting))]
        public IHttpActionResult DeleteServoSetting(int id)
        {
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            if (servoSetting == null)
            {
                return NotFound();
            }

            db.ServoSetting.Remove(servoSetting);
            db.SaveChanges();

            return Ok(servoSetting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServoSettingExists(int id)
        {
            return db.ServoSetting.Count(e => e.ServoSettingID == id) > 0;
        }
    }
}