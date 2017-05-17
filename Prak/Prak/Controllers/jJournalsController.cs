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
    public class jJournalsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: jJournals
        public ActionResult Index()
        {
            var jJournal = db.jJournal.Include(j => j.AspNetUsers).Include(j => j.hEventType).Include(j => j.jWorkList);
            return View(jJournal.ToList());
        }

        // GET: jJournals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jJournal jJournal = db.jJournal.Find(id);
            if (jJournal == null)
            {
                return HttpNotFound();
            }
            return View(jJournal);
        }

        // GET: jJournals/Create
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.EventTypeId = new SelectList(db.hEventType, "EventTypeId", "Description");
            ViewBag.WorkListId = new SelectList(db.jWorkList, "WorkListId", "PersonExecId");
            return View();
        }

        // POST: jJournals/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Date,EventTypeId,WorkListId,Description,JournalId,PersonId,QueryID")] jJournal jJournal)
        {
            if (ModelState.IsValid)
            {
                db.jJournal.Add(jJournal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Email", jJournal.PersonId);
            ViewBag.EventTypeId = new SelectList(db.hEventType, "EventTypeId", "Description", jJournal.EventTypeId);
            ViewBag.WorkListId = new SelectList(db.jWorkList, "WorkListId", "PersonExecId", jJournal.WorkListId);
            return View(jJournal);
        }

        // GET: jJournals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jJournal jJournal = db.jJournal.Find(id);
            if (jJournal == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Email", jJournal.PersonId);
            ViewBag.EventTypeId = new SelectList(db.hEventType, "EventTypeId", "Description", jJournal.EventTypeId);
            ViewBag.WorkListId = new SelectList(db.jWorkList, "WorkListId", "PersonExecId", jJournal.WorkListId);
            return View(jJournal);
        }

        // POST: jJournals/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Date,EventTypeId,WorkListId,Description,JournalId,PersonId,QueryID")] jJournal jJournal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jJournal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Email", jJournal.PersonId);
            ViewBag.EventTypeId = new SelectList(db.hEventType, "EventTypeId", "Description", jJournal.EventTypeId);
            ViewBag.WorkListId = new SelectList(db.jWorkList, "WorkListId", "PersonExecId", jJournal.WorkListId);
            return View(jJournal);
        }

        // GET: jJournals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jJournal jJournal = db.jJournal.Find(id);
            if (jJournal == null)
            {
                return HttpNotFound();
            }
            return View(jJournal);
        }

        // POST: jJournals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jJournal jJournal = db.jJournal.Find(id);
            db.jJournal.Remove(jJournal);
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
