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

namespace sep4.Controllers
{
    public class APIDatapointsController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: api/APIDatapoints
        public IQueryable<Datapoint> GetDatapoint()
        {
            return db.Datapoint;
        }

        // GET: api/APIDatapoints/5
        [ResponseType(typeof(Datapoint))]
        public IHttpActionResult GetDatapoint(int id)
        {
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return NotFound();
            }

            return Ok(datapoint);
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