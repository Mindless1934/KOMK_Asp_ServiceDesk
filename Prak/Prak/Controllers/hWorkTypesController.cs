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
    public class hWorkTypesController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hWorkTypes
        public ActionResult Index()
        {
            var hWorkType = db.hWorkType.Include(h => h.hSkill);
            return View(hWorkType.ToList());
        }

        // GET: hWorkTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hWorkType hWorkType = db.hWorkType.Find(id);
            if (hWorkType == null)
            {
                return HttpNotFound();
            }
            return View(hWorkType);
        }

        // GET: hWorkTypes/Create
        public ActionResult Create()
        {
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description");
            return View();
        }

        // POST: hWorkTypes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkTypeId,Description,SkillId")] hWorkType hWorkType)
        {
            if (ModelState.IsValid)
            {
                db.hWorkType.Add(hWorkType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", hWorkType.SkillId);
            return View(hWorkType);
        }

        // GET: hWorkTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hWorkType hWorkType = db.hWorkType.Find(id);
            if (hWorkType == null)
            {
                return HttpNotFound();
            }
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", hWorkType.SkillId);
            return View(hWorkType);
        }

        // POST: hWorkTypes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkTypeId,Description,SkillId")] hWorkType hWorkType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hWorkType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", hWorkType.SkillId);
            return View(hWorkType);
        }

        // GET: hWorkTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hWorkType hWorkType = db.hWorkType.Find(id);
            if (hWorkType == null)
            {
                return HttpNotFound();
            }
            return View(hWorkType);
        }

        // POST: hWorkTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hWorkType hWorkType = db.hWorkType.Find(id);
            db.hWorkType.Remove(hWorkType);
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
