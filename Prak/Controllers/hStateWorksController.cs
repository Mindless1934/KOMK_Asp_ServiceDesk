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
    public class hStateWorksController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hStateWorks
        public ActionResult Index()
        {
            return View(db.hStateWork.ToList());
        }

        // GET: hStateWorks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hStateWork hStateWork = db.hStateWork.Find(id);
            if (hStateWork == null)
            {
                return HttpNotFound();
            }
            return View(hStateWork);
        }

        // GET: hStateWorks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hStateWorks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StateWorkId,Description")] hStateWork hStateWork)
        {
            if (ModelState.IsValid)
            {
                db.hStateWork.Add(hStateWork);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hStateWork);
        }

        // GET: hStateWorks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hStateWork hStateWork = db.hStateWork.Find(id);
            if (hStateWork == null)
            {
                return HttpNotFound();
            }
            return View(hStateWork);
        }

        // POST: hStateWorks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateWorkId,Description")] hStateWork hStateWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hStateWork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hStateWork);
        }

        // GET: hStateWorks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hStateWork hStateWork = db.hStateWork.Find(id);
            if (hStateWork == null)
            {
                return HttpNotFound();
            }
            return View(hStateWork);
        }

        // POST: hStateWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hStateWork hStateWork = db.hStateWork.Find(id);
            db.hStateWork.Remove(hStateWork);
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
