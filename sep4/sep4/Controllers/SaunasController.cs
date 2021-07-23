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
    public class SaunasController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: Saunas
        public ActionResult Index()
        {
            var sauna = db.Sauna.Include(s => s.Establishment);
            return View(sauna.ToList());
        }

        // GET: Saunas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return HttpNotFound();
            }
            return View(sauna);
        }

        // GET: Saunas/Create
        public ActionResult Create()
        {
            ViewBag.EstablishmentID = new SelectList(db.Establishment, "EstablishmentID", "Name");
            return View();
        }

        // POST: Saunas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SaunaID,EstablishmentID,TemperatureThreshold,CO2Threshold,HumidityThreshold")] Sauna sauna)
        {
            if (ModelState.IsValid)
            {
                db.Sauna.Add(sauna);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstablishmentID = new SelectList(db.Establishment, "EstablishmentID", "Name", sauna.EstablishmentID);
            return View(sauna);
        }

        // GET: Saunas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstablishmentID = new SelectList(db.Establishment, "EstablishmentID", "Name", sauna.EstablishmentID);
            return View(sauna);
        }

        // POST: Saunas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SaunaID,EstablishmentID,TemperatureThreshold,CO2Threshold,HumidityThreshold")] Sauna sauna)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sauna).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstablishmentID = new SelectList(db.Establishment, "EstablishmentID", "Name", sauna.EstablishmentID);
            return View(sauna);
        }

        // GET: Saunas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sauna sauna = db.Sauna.Find(id);
            if (sauna == null)
            {
                return HttpNotFound();
            }
            return View(sauna);
        }

        // POST: Saunas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sauna sauna = db.Sauna.Find(id);
            db.Sauna.Remove(sauna);
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
