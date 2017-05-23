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
    public class hSkillsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: hSkills
        public ActionResult Index()
        {
            return View(db.hSkill.ToList());
        }

        // GET: hSkills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hSkill hSkill = db.hSkill.Find(id);
            if (hSkill == null)
            {
                return HttpNotFound();
            }
            return View(hSkill);
        }

        // GET: hSkills/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hSkills/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillId,Description")] hSkill hSkill)
        {
            if (ModelState.IsValid)
            {
                db.hSkill.Add(hSkill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hSkill);
        }

        // GET: hSkills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hSkill hSkill = db.hSkill.Find(id);
            if (hSkill == null)
            {
                return HttpNotFound();
            }
            return View(hSkill);
        }

        // POST: hSkills/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillId,Description")] hSkill hSkill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hSkill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hSkill);
        }

        // GET: hSkills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hSkill hSkill = db.hSkill.Find(id);
            if (hSkill == null)
            {
                return HttpNotFound();
            }
            return View(hSkill);
        }

        // POST: hSkills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hSkill hSkill = db.hSkill.Find(id);
            db.hSkill.Remove(hSkill);
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
