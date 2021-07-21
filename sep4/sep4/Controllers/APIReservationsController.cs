﻿using System;
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
    public class APIReservationsController : ApiController
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: api/APIReservations
        [ResponseType(typeof(List<ReservationDTO>))]
        public IHttpActionResult GetReservations()
        {
            List<ReservationDTO> reservationDTOs = new List<ReservationDTO>();
            foreach (var reservation in db.Reservation.ToList())
            {
                ReservationDTO reservationDTO = new ReservationDTO(reservation.UserID, reservation.SaunaID, reservation.FromDateTime, reservation.ToDateTime);
                reservationDTOs.Add(reservationDTO);
            }

            return Ok(reservationDTOs);
        }

        // GET: api/APIReservations/5
        [ResponseType(typeof(ReservationDTO))]
        public IHttpActionResult GetReservation(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ReservationDTO reservationDTO = new ReservationDTO(reservation.UserID, reservation.SaunaID, reservation.FromDateTime, reservation.ToDateTime);

            return Ok(reservationDTO);
        }

        // PUT: api/APIReservations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(int id, Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservation.UserID)
            {
                return BadRequest();
            }

            db.Entry(reservation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/APIReservations
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reservation.Add(reservation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReservationExists(reservation.UserID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reservation.UserID }, reservation);
        }

        // DELETE: api/APIReservations/5
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            db.Reservation.Remove(reservation);
            db.SaveChanges();

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return db.Reservation.Count(e => e.UserID == id) > 0;
        }
    }
}