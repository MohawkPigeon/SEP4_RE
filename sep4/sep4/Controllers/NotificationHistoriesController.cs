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
    public class NotificationHistoriesController : Controller
    {
        private sep4_dbEntities1 db = new sep4_dbEntities1();

        // GET: NotificationHistories
        public ActionResult Index()
        {
            var notificationHistory = db.NotificationHistory.Include(n => n.User);
            return View(notificationHistory.ToList());
        }

        // GET: NotificationHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            if (notificationHistory == null)
            {
                return HttpNotFound();
            }
            return View(notificationHistory);
        }

        // GET: NotificationHistories/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username");
            return View();
        }

        // POST: NotificationHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NotificationID,UserID,DateTime")] NotificationHistory notificationHistory)
        {
            if (ModelState.IsValid)
            {
                db.NotificationHistory.Add(notificationHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", notificationHistory.UserID);
            return View(notificationHistory);
        }

        // GET: NotificationHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            if (notificationHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", notificationHistory.UserID);
            return View(notificationHistory);
        }

        // POST: NotificationHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NotificationID,UserID,DateTime")] NotificationHistory notificationHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notificationHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "Username", notificationHistory.UserID);
            return View(notificationHistory);
        }

        // GET: NotificationHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            if (notificationHistory == null)
            {
                return HttpNotFound();
            }
            return View(notificationHistory);
        }

        // POST: NotificationHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotificationHistory notificationHistory = db.NotificationHistory.Find(id);
            db.NotificationHistory.Remove(notificationHistory);
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
