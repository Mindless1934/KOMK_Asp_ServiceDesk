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
    public class jSkillListsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: jSkillLists
        public ActionResult Index()
        {
            var jSkillList = db.jSkillList.Include(j => j.hSkill).Include(j => j.AspNetUsers);
            return View(jSkillList.ToList());
        }

        // GET: jSkillLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jSkillList jSkillList = db.jSkillList.Find(id);
            if (jSkillList == null)
            {
                return HttpNotFound();
            }
            return View(jSkillList);
        }

        // GET: jSkillLists/Create
        public ActionResult Create()
        {
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description");
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio");
            return View();
        }

        // POST: jSkillLists/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SkillListId,SkillId,PersonId")] jSkillList jSkillList)
        {
            if (ModelState.IsValid)
            {
                db.jSkillList.Add(jSkillList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", jSkillList.SkillId);
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jSkillList.PersonId);
            return View(jSkillList);
        }

        // GET: jSkillLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jSkillList jSkillList = db.jSkillList.Find(id);
            if (jSkillList == null)
            {
                return HttpNotFound();
            }
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", jSkillList.SkillId);
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jSkillList.PersonId);
            return View(jSkillList);
        }

        // POST: jSkillLists/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SkillListId,SkillId,PersonId")] jSkillList jSkillList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jSkillList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SkillId = new SelectList(db.hSkill, "SkillId", "Description", jSkillList.SkillId);
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jSkillList.PersonId);
            return View(jSkillList);
        }

        // GET: jSkillLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jSkillList jSkillList = db.jSkillList.Find(id);
            if (jSkillList == null)
            {
                return HttpNotFound();
            }
            return View(jSkillList);
        }

        // POST: jSkillLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jSkillList jSkillList = db.jSkillList.Find(id);
            db.jSkillList.Remove(jSkillList);
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
