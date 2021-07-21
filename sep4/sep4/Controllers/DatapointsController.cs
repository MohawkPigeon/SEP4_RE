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
    public class DatapointsController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: Datapoints
        public ActionResult Index()
        {
            var datapoint = db.Datapoint.Include(d => d.Sauna);
            return View(datapoint.ToList());
        }

        // GET: Datapoints/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return HttpNotFound();
            }
            return View(datapoint);
        }

        // GET: Datapoints/Create
        public ActionResult Create()
        {
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold");
            return View();
        }

        // POST: Datapoints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DatapointID,SaunaID,DateTime,Temperature,Co2,Humidity,ServoSettingAtTime")] Datapoint datapoint)
        {
            if (ModelState.IsValid)
            {
                db.Datapoint.Add(datapoint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", datapoint.SaunaID);
            return View(datapoint);
        }

        // GET: Datapoints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", datapoint.SaunaID);
            return View(datapoint);
        }

        // POST: Datapoints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DatapointID,SaunaID,DateTime,Temperature,Co2,Humidity,ServoSettingAtTime")] Datapoint datapoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datapoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", datapoint.SaunaID);
            return View(datapoint);
        }

        // GET: Datapoints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Datapoint datapoint = db.Datapoint.Find(id);
            if (datapoint == null)
            {
                return HttpNotFound();
            }
            return View(datapoint);
        }

        // POST: Datapoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Datapoint datapoint = db.Datapoint.Find(id);
            db.Datapoint.Remove(datapoint);
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
