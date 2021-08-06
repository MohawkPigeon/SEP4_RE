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
    public class EstablishmentsController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: Establishments
        public ActionResult Index()
        {
            return View(db.Establishment.ToList());
        }

        // GET: Establishments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Establishment establishment = db.Establishment.Find(id);
            if (establishment == null)
            {
                return HttpNotFound();
            }
            return View(establishment);
        }

        // GET: Establishments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Establishments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EstablishmentID,Name")] Establishment establishment)
        {
            if (ModelState.IsValid)
            {
                establishment.DateTime = DateTime.Now;
                db.Establishment.Add(establishment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(establishment);
        }

        // GET: Establishments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Establishment establishment = db.Establishment.Find(id);
            if (establishment == null)
            {
                return HttpNotFound();
            }
            return View(establishment);
        }

        // POST: Establishments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EstablishmentID,Name")] Establishment establishment)
        {
            if (ModelState.IsValid)
            {
                establishment.DateTime = DateTime.Now;
                db.Entry(establishment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(establishment);
        }

        // GET: Establishments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Establishment establishment = db.Establishment.Find(id);
            if (establishment == null)
            {
                return HttpNotFound();
            }
            return View(establishment);
        }

        // POST: Establishments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Establishment establishment = db.Establishment.Find(id);

            StageEstablishmentDIM stageEstablishment = db.StageEstablishmentDIM.Where(se => se.EstablishmentID == establishment.EstablishmentID && se.ValidTo > DateTime.Now).FirstOrDefault();
            stageEstablishment.ValidTo = DateTime.Now.AddDays(-1);

            db.Establishment.Remove(establishment);
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
