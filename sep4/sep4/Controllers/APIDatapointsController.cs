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
    public class APIDatapointsController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APIDatapoints
        [ResponseType(typeof(List<DatapointDTO>))]
        public IHttpActionResult GetDatapoints()
        {
            List<DatapointDTO> datapointDTOs = new List<DatapointDTO>();
            foreach (var datapoint in db.Datapoint.ToList())
            {
                DatapointDTO datapointDTO = new DatapointDTO(datapoint.DatapointID, datapoint.SaunaID, datapoint.DateTime, datapoint.Temperature, datapoint.Co2, datapoint.Humidity, datapoint.ServoSettingAtTime);
                datapointDTOs.Add(datapointDTO);
            }

            return Ok(datapointDTOs);
        }

        // GET: api/APIDatapoints/5
        [ResponseType(typeof(DatapointDTO))]
        public IHttpActionResult GetDatapoint(int id)
        {
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return NotFound();
            }
            DatapointDTO datapointDTO = new DatapointDTO(datapoint.DatapointID, datapoint.SaunaID, datapoint.DateTime, datapoint.Temperature, datapoint.Co2, datapoint.Humidity, datapoint.ServoSettingAtTime);


            return Ok(datapointDTO);
        }

        // PUT: api/APIDatapoints/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDatapoint(int id, Datapoint datapoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != datapoint.DatapointID)
            {
                return BadRequest();
            }

            db.Entry(datapoint).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatapointExists(id))
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

        // POST: api/APIDatapoints
        [ResponseType(typeof(Datapoint))]
        public IHttpActionResult PostDatapoint(Datapoint datapoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            return CreatedAtRoute("DefaultApi", new { id = datapoint.DatapointID }, datapoint);
        }

        // DELETE: api/APIDatapoints/5
        [ResponseType(typeof(Datapoint))]
        public IHttpActionResult DeleteDatapoint(int id)
        {
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return NotFound();
            }

            db.Datapoint.Remove(datapoint);
            db.SaveChanges();

            return Ok(datapoint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DatapointExists(int id)
        {
            return db.Datapoint.Count(e => e.DatapointID == id) > 0;
        }
    }
}