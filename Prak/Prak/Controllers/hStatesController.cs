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
    public class hStatesController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hStates
        public ActionResult Index()
        {
            return View(db.hState.ToList());
        }

        // GET: hStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hState hState = db.hState.Find(id);
            if (hState == null)
            {
                return HttpNotFound();
            }
            return View(hState);
        }

        // GET: hStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hStates/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StateId,Description")] hState hState)
        {
            if (ModelState.IsValid)
            {
                db.hState.Add(hState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hState);
        }

        // GET: hStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hState hState = db.hState.Find(id);
            if (hState == null)
            {
                return HttpNotFound();
            }
            return View(hState);
        }

        // POST: hStates/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateId,Description")] hState hState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hState);
        }

        // GET: hStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hState hState = db.hState.Find(id);
            if (hState == null)
            {
                return HttpNotFound();
            }
            return View(hState);
        }

        // POST: hStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hState hState = db.hState.Find(id);
            db.hState.Remove(hState);
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
