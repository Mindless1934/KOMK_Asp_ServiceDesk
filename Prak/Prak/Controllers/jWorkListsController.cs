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
    public class jWorkListsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: jWorkLists
        public ActionResult Index()
        {
            var jWorkList = db.jWorkList.Include(j => j.AspNetUsers).Include(j => j.hStateWork).Include(j => j.hWorkType).Include(j => j.jQuery);
            return View(jWorkList.ToList());
        }

        // GET: jWorkLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jWorkList jWorkList = db.jWorkList.Find(id);
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            return View(jWorkList);
        }

        // GET: jWorkLists/Create
        public ActionResult Create()
        {

            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description");
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description");
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text");
            return View();
        }

        // POST: jWorkLists/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkListId,DateIn,DateOut,DateModifcation,Deadline,QueryId,WorkTypeId,PersonExecId,StateWorkId,Verification,Comment")] jWorkList jWorkList)
        {
            if (ModelState.IsValid)
            {
                jWorkList.DateIn = DateTime.Parse(DateTime.Today.ToShortDateString());
                jWorkList.DateModifcation = DateTime.Now;
                jWorkList.StateWorkId = 4;
                jWorkList.Verification = false;
                string com = "-" + jWorkList.Comment + " "+User.Identity.Name + " " +DateTime.Now.ToString();
                jWorkList.Comment = com;
                db.jWorkList.Add(jWorkList);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // GET: jWorkLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jWorkList jWorkList = db.jWorkList.Find(id);
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.WorkTest = ViewBag.WorkTypeId;
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // POST: jWorkLists/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkListId,DateIn,DateOut,DateModifcation,Deadline,QueryId,WorkTypeId,PersonExecId,StateWorkId,Verification,Comment")] jWorkList jWorkList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jWorkList).State = EntityState.Modified;             
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // GET: jWorkLists/ChangeStateWork/5
        public ActionResult ChangeStateWork(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jWorkList jWorkList = db.jWorkList.Find(id);
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);            
            return View(jWorkList);
        }

        // POST: jWorkLists/ChangeStateWork/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStateWork([Bind(Include = "WorkListId,DateIn,DateOut,DateModifcation,Deadline,QueryId,WorkTypeId,PersonExecId,StateWorkId,Verification,Comment")] jWorkList jWorkList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jWorkList).State = EntityState.Modified;                
                string com = "  -" + Request.Form["addCom"]+  " " + User.Identity.Name + " " + DateTime.Now.ToString();
                jWorkList.Comment = jWorkList.Comment + Environment.NewLine + com;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "UserName", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // GET: jWorkLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jWorkList jWorkList = db.jWorkList.Find(id);
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            return View(jWorkList);
        }

        // POST: jWorkLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jWorkList jWorkList = db.jWorkList.Find(id);
            db.jWorkList.Remove(jWorkList);
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
