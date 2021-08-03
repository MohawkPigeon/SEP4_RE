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
    public class APIEstablishmentsController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APIEstablishments
        [ResponseType(typeof(List<EstablishmentDTO>))]
        public IHttpActionResult GetEstablishments()
        {
            List<EstablishmentDTO> establishmentDTOs = new List<EstablishmentDTO>();
            foreach (var establishment in db.Establishment.ToList())
            {
                EstablishmentDTO establishmentDTO = new EstablishmentDTO(establishment.EstablishmentID, establishment.Name);
                establishmentDTOs.Add(establishmentDTO);
            }

            return Ok(establishmentDTOs);
        }

        // GET: api/APIEstablishments/5
        [ResponseType(typeof(EstablishmentDTO))]
        public IHttpActionResult GetEstablishment(int id)
        {
            Establishment establishment = db.Establishment.Find(id);
            if (establishment == null)
            {
                return NotFound();
            }
            EstablishmentDTO establishmentDTO = new EstablishmentDTO(establishment.EstablishmentID, establishment.Name);

            return Ok(establishmentDTO);
        }

        // PUT: api/APIEstablishments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstablishment(int id, Establishment establishment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != establishment.EstablishmentID)
            {
                return BadRequest();
            }

            db.Entry(establishment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstablishmentExists(id))
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

        // POST: api/APIEstablishments
        [ResponseType(typeof(Establishment))]
        public IHttpActionResult PostEstablishment(Establishment establishment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Establishment.Add(establishment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EstablishmentExists(establishment.EstablishmentID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = establishment.EstablishmentID }, establishment);
        }

        // DELETE: api/APIEstablishments/5
        [ResponseType(typeof(Establishment))]
        public IHttpActionResult DeleteEstablishment(int id)
        {
            Establishment establishment = db.Establishment.Find(id);
            if (establishment == null)
            {
                return NotFound();
            }
            StageEstablishmentDIM stageEstablishment = db.StageEstablishmentDIM.Where(se => se.EstablishmentID == establishment.EstablishmentID && se.ValidTo > DateTime.Now).FirstOrDefault();
            stageEstablishment.ValidTo = DateTime.Now.AddDays(-1);

            db.Establishment.Remove(establishment);
            db.SaveChanges();

            return Ok(establishment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstablishmentExists(int id)
        {
            return db.Establishment.Count(e => e.EstablishmentID == id) > 0;
        }
    }
}