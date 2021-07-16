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
    public class APINotificationHistoriesController : ApiController
    {
        private DatabaseEntities db = new DatabaseEntities();

        // GET: api/APINotificationHistories
        public IQueryable<NotificationHistory> GetNotificationHistory()
        {
            return db.NotificationHistory;
        }

        // GET: api/APINotificationHistories/5
        [ResponseType(typeof(NotificationHistory))]
        public IHttpActionResult GetNotificationHistory(int id)
        {
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            if (notificationHistory == null)
            {
                return NotFound();
            }

            return Ok(notificationHistory);
        }

        // PUT: api/APINotificationHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotificationHistory(int id, NotificationHistory notificationHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notificationHistory.NotificationID)
            {
                return BadRequest();
            }

            db.Entry(notificationHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationHistoryExists(id))
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

        // POST: api/APINotificationHistories
        [ResponseType(typeof(NotificationHistory))]
        public IHttpActionResult PostNotificationHistory(NotificationHistory notificationHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NotificationHistory.Add(notificationHistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (NotificationHistoryExists(notificationHistory.NotificationID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = notificationHistory.NotificationID }, notificationHistory);
        }

        // DELETE: api/APINotificationHistories/5
        [ResponseType(typeof(NotificationHistory))]
        public IHttpActionResult DeleteNotificationHistory(int id)
        {
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            if (notificationHistory == null)
            {
                return NotFound();
            }

            db.NotificationHistory.Remove(notificationHistory);
            db.SaveChanges();

            return Ok(notificationHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationHistoryExists(int id)
        {
            return db.NotificationHistory.Count(e => e.NotificationID == id) > 0;
        }
    }
}