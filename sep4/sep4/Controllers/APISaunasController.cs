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
    public class APISaunasController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: api/APISaunas
        public IQueryable<Sauna> GetSauna()
        {
            return db.Sauna;
        }

        // GET: api/APISaunas/5
        [ResponseType(typeof(Sauna))]
        public IHttpActionResult GetSauna(int id)
        {
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return NotFound();
            }

            return Ok(sauna);
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

        // POST: api/APISaunas
        [ResponseType(typeof(Sauna))]
        public IHttpActionResult PostSauna(Sauna sauna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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