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
    public class hPersonsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hPersons
        public ActionResult Index()
        {
            var hPerson = db.hPerson.Include(h => h.hAccess);
            return View(hPerson.ToList());
        }

        // GET: hPersons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hPerson hPerson = db.hPerson.Find(id);
            //List<jQuery> jQuery = 
            if (hPerson == null)
            {
                return HttpNotFound();
            }
            return View(hPerson);
        }

        // GET: hPersons/Create
        public ActionResult Create()
        {
            ViewBag.AccessId = new SelectList(db.hAccess, "AccessID", "Description");
            return View();
        }

        // POST: hPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,FIO,AccessId")] hPerson hPerson)
        {
            if (ModelState.IsValid)
            {
                db.hPerson.Add(hPerson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccessId = new SelectList(db.hAccess, "AccessID", "Description", hPerson.AccessId);
            return View(hPerson);
        }

        // GET: hPersons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hPerson hPerson = db.hPerson.Find(id);
            if (hPerson == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccessId = new SelectList(db.hAccess, "AccessID", "Description", hPerson.AccessId);
            return View(hPerson);
        }

        // POST: hPersons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,FIO,AccessId")] hPerson hPerson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hPerson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccessId = new SelectList(db.hAccess, "AccessID", "Description", hPerson.AccessId);
            return View(hPerson);
        }

        // GET: hPersons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hPerson hPerson = db.hPerson.Find(id);
            if (hPerson == null)
            {
                return HttpNotFound();
            }
            return View(hPerson);
        }

        // POST: hPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hPerson hPerson = db.hPerson.Find(id);
            db.hPerson.Remove(hPerson);
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
