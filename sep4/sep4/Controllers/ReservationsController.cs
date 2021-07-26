using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sep4;

namespace sep4.Controllers
{
    public class ReservationsController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservation = db.Reservation.Include(r => r.Sauna).Include(r => r.User);
            return View(reservation.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? SaunaID, int? UserID)
        {
            if (SaunaID == null || UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(SaunaID, UserID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "TemperatureThreshold");
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,SaunaID,FromDateTime,ToDateTime")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservation.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "TemperatureThreshold", reservation.SaunaID);
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? SaunaID, int? UserID)
        {
            if (SaunaID == null || UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(SaunaID, UserID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "TemperatureThreshold", reservation.SaunaID);
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", reservation.UserID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,SaunaID,FromDateTime,ToDateTime")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "TemperatureThreshold", reservation.SaunaID);
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", reservation.UserID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? SaunaID, int? UserID)
        {
            if (SaunaID == null || UserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(SaunaID, UserID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? SaunaID, int? UserID)
        {
            Reservation reservation = db.Reservation.Find(SaunaID, UserID);
            db.Reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
