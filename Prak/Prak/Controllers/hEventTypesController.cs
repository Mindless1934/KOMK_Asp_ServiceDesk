using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prak.Models;

namespace Prak.Controllers
{
    public class hEventTypesController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hEventTypes
        public ActionResult Index()
        {
            return View(db.hEventType.ToList());
        }

        // GET: hEventTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hEventType hEventType = db.hEventType.Find(id);
            if (hEventType == null)
            {
                return HttpNotFound();
            }
            return View(hEventType);
        }

        // GET: hEventTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hEventTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventTypeId,Description")] hEventType hEventType)
        {
            if (ModelState.IsValid)
            {
                db.hEventType.Add(hEventType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hEventType);
        }

        // GET: hEventTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hEventType hEventType = db.hEventType.Find(id);
            if (hEventType == null)
            {
                return HttpNotFound();
            }
            return View(hEventType);
        }

        // POST: hEventTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventTypeId,Description")] hEventType hEventType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hEventType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hEventType);
        }

        // GET: hEventTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hEventType hEventType = db.hEventType.Find(id);
            if (hEventType == null)
            {
                return HttpNotFound();
            }
            return View(hEventType);
        }

        // POST: hEventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hEventType hEventType = db.hEventType.Find(id);
            db.hEventType.Remove(hEventType);
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
