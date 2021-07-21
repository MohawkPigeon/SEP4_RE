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
    public class ServoSettingsController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: ServoSettings
        public ActionResult Index()
        {
            var servoSetting = db.ServoSetting.Include(s => s.Sauna);
            return View(servoSetting.ToList());
        }

        // GET: ServoSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            if (servoSetting == null)
            {
                return HttpNotFound();
            }
            return View(servoSetting);
        }

        // GET: ServoSettings/Create
        public ActionResult Create()
        {
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold");
            return View();
        }

        // POST: ServoSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServoSettingID,SaunaID,Datetime,ServoSetting1")] ServoSetting servoSetting)
        {
            if (ModelState.IsValid)
            {
                db.ServoSetting.Add(servoSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", servoSetting.SaunaID);
            return View(servoSetting);
        }

        // GET: ServoSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            if (servoSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", servoSetting.SaunaID);
            return View(servoSetting);
        }

        // POST: ServoSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServoSettingID,SaunaID,Datetime,ServoSetting1")] ServoSetting servoSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servoSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SaunaID = new SelectList(db.Sauna, "SaunaID", "Threshold", servoSetting.SaunaID);
            return View(servoSetting);
        }

        // GET: ServoSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            if (servoSetting == null)
            {
                return HttpNotFound();
            }
            return View(servoSetting);
        }

        // POST: ServoSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServoSetting servoSetting = db.ServoSetting.Find(id);
            db.ServoSetting.Remove(servoSetting);
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
