﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Prak.Models;
using Microsoft.AspNet.Identity;

namespace Prak.Controllers
{
    public class jWorkListsController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: jWorkLists
        public ActionResult Index()
        {
            var jWorkList = db.jWorkList.Include(j => j.AspNetUsers).Include(j => j.hStateWork).Include(j => j.hWorkType).Include(j => j.jQuery);
            AspNetUsers userNow = db.AspNetUsers.Find(User.Identity.GetUserId());
            AspNetUserRoles usrol = db.AspNetUserRoles.Where(m => m.UserId == userNow.Id).First();
            AspNetRoles rol = db.AspNetRoles.Where(m => m.Id == usrol.RoleId).First();
            switch (rol.Name)
            {
                case "Admin": return View(jWorkList.ToList());
                case "Worker": return View(jWorkList.Where(m => m.PersonExecId == userNow.Id).ToList());
            }
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
        
        public ActionResult GetNewDDList(int id)
        {
            int idSkil = db.hWorkType.Where(i => i.WorkTypeId == id).First().SkillId;
            var persnFormjSkillList = db.jSkillList.Where(m => m.SkillId == idSkil);
            var aspNetUsers = db.AspNetUsers;
            List<AspNetUsers> listpolz = new List<AspNetUsers>();
            foreach (jSkillList skillLis in persnFormjSkillList)
            {
                listpolz.Add(aspNetUsers.Where(m => m.Id == skillLis.PersonId).First());
            }
            ViewBag.PersonExecId = new SelectList(listpolz, "Id", "Fio");
            return PartialView("GetNewDDList");
        }

        public List<AspNetUsers> GetListWromWT(int id)
        {
            int idSkil = db.hWorkType.Where(i => i.WorkTypeId == id).First().SkillId;
            var persnFormjSkillList = db.jSkillList.Where(m => m.SkillId == idSkil);
            var aspNetUsers = db.AspNetUsers;
            List<AspNetUsers> listpolz = new List<AspNetUsers>();
            foreach (jSkillList skillLis in persnFormjSkillList)
            {
                listpolz.Add(aspNetUsers.Where(m => m.Id == skillLis.PersonId).First());
            }
            return listpolz;
        }

        public List<jQuery> GetOpenQuery()
        {       
            var allQuery = db.jQuery;
            List<jQuery> opQuer = new List<jQuery>();
            foreach (jQuery quer in allQuery)
            {
                if (quer.StateId != db.hState.First(m => m.Description == "Выполнена").StateId && quer.StateId != db.hState.First(m => m.Description == "Отклонена").StateId) {
                    opQuer.Add(quer);
                }
            }
            return opQuer;
        }
        // GET: jWorkLists/Create
        public ActionResult Create()
        {

            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description");            
            ViewBag.PersonExecId = new SelectList(GetListWromWT(db.hWorkType.First().WorkTypeId), "Id", "Fio");
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description");            
            ViewBag.QueryId = new SelectList(GetOpenQuery(), "QueryId", "Text");
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
                jWorkList.DateModifcation = jWorkList.DateModifcation.AddMilliseconds(-jWorkList.DateModifcation.Millisecond);
                DateTime dmd = jWorkList.DateModifcation;
                jWorkList.StateWorkId = db.hStateWork.First(m => m.Description == "Ожидает").StateWorkId;
                jWorkList.Verification = false;
                if (jWorkList.Comment != null)
                {
                    string com = "-" + jWorkList.Comment + " " + User.Identity.Name + " " + DateTime.Now.ToString();
                    jWorkList.Comment = com;
                }
                db.jWorkList.Add(jWorkList);
                db.SaveChanges();

                jJournal jJur = new jJournal();
                string dmdstr = dmd.ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                DateTime dmdn = DateTime.Parse(dmdstr);
                db = new KOMK_Main_v2Entities();
                jWorkList jW = db.jWorkList.First(m => m.DateModifcation == dmdn);

                jJur.Date = DateTime.Now;
                jJur.EventTypeId = db.hEventType.First(m => m.Description == "Создание работы").EventTypeId;
                jJur.WorkListId = jW.WorkListId;
                jJur.PersonId = User.Identity.GetUserId();
                jJur.QueryID = jW.QueryId;
                if (jW.Comment != null)
                {
                    jJur.Description = " Коментарий перед работой: " + jW.Comment;
                }
                else
                {
                    jJur.Description = "Комментария перед работой не было";
                }

                db.jJournal.Add(jJur);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
            TempData["oldState"] = jWorkList.StateWorkId;
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
                if (jWorkList.StateWorkId == db.hStateWork.First(m => m.Description == "Выполнена").StateWorkId || jWorkList.StateWorkId == db.hStateWork.First(m => m.Description == "Отклонена").StateWorkId)
                {
                    jWorkList.DateOut = DateTime.Parse(DateTime.Today.ToShortDateString());
                }
                if (Convert.ToInt32(TempData["oldState"]) != jWorkList.StateWorkId)
                {
                    jJournal jJur = new jJournal();
                    jJur.Date = DateTime.Now;
                    jJur.EventTypeId = db.hEventType.First(m => m.Description == "Смена статуса работы").EventTypeId;
                    jJur.WorkListId = jWorkList.WorkListId;
                    jJur.PersonId = User.Identity.GetUserId();
                    jJur.QueryID = jWorkList.QueryId;
                    hStateWork oldst = db.hStateWork.Find(Convert.ToInt32(TempData["oldState"]));
                    hStateWork newst = db.hStateWork.Find(jWorkList.StateWorkId);
                    jJur.Description = "c " + oldst.Description + " на " + newst.Description;
                    CloseQuery(jWorkList.QueryId);
                    db.jJournal.Add(jJur);
                }
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // GET: jWorkLists/ChangeStateWork/5
        public ActionResult Comments(int? id)
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
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
        public ActionResult Comments([Bind(Include = "WorkListId,DateIn,DateOut,DateModifcation,Deadline,QueryId,WorkTypeId,PersonExecId,StateWorkId,Verification,Comment")] jWorkList jWorkList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jWorkList).State = EntityState.Modified;
                if (Request.Form["addCom"] != "")
                {
                    string com = "  -" + Request.Form["addCom"] + " " + User.Identity.Name + " " + DateTime.Now.ToString();
                    jWorkList.Comment = jWorkList.Comment + "\n" + com;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // GET: jWorkLists/ChangeWorker/5
        public ActionResult ChangeWorker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jWorkList jWorkList = db.jWorkList.Find(id);
            TempData["oldWork"] = jWorkList.PersonExecId;
            if (jWorkList == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonExecId = new SelectList(GetListWromWT(db.hWorkType.Find(jWorkList.WorkTypeId).WorkTypeId ), "Id", "Fio", jWorkList.PersonExecId);
            ViewBag.StateWorkId = new SelectList(db.hStateWork, "StateWorkId", "Description", jWorkList.StateWorkId);
            ViewBag.WorkTypeId = new SelectList(db.hWorkType, "WorkTypeId", "Description", jWorkList.WorkTypeId);
            ViewBag.QueryId = new SelectList(db.jQuery, "QueryId", "Text", jWorkList.QueryId);
            return View(jWorkList);
        }

        // POST: jWorkLists/ChangeWorker/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeWorker([Bind(Include = "WorkListId,DateIn,DateOut,DateModifcation,Deadline,QueryId,WorkTypeId,PersonExecId,StateWorkId,Verification,Comment")] jWorkList jWorkList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jWorkList).State = EntityState.Modified;
                if (TempData["oldWork"].ToString() != jWorkList.PersonExecId) {
                    jJournal jJur = new jJournal();
                    jJur.Date = DateTime.Now;
                    jJur.EventTypeId = db.hEventType.First(m => m.Description == "Смена исполнителя").EventTypeId;
                    jJur.WorkListId = jWorkList.WorkListId;
                    jJur.PersonId = User.Identity.GetUserId();
                    jJur.QueryID = jWorkList.QueryId;
                    AspNetUsers oldst = db.AspNetUsers.Find(TempData["oldWork"]);
                    AspNetUsers newst = db.AspNetUsers.Find(jWorkList.PersonExecId);
                    jJur.Description = "c " + oldst.Fio + " на " + newst.Fio;
                    db.jJournal.Add(jJur);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonExecId = new SelectList(db.AspNetUsers, "Id", "Fio", jWorkList.PersonExecId);
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
        public void CloseQuery(int idQury)
        {
            var workByQuery = db.jWorkList.Where(m => m.QueryId == idQury);
            Boolean close = true;       
            foreach (jWorkList work in workByQuery)
            {
                if (work.StateWorkId != db.hStateWork.First(m => m.Description == "Выполнена").StateWorkId)
                {
                    close = false;
                }
            }
            if (close)
            {
                jQuery jQuery = db.jQuery.Find(idQury);
                string oldst = db.hState.Find(jQuery.StateId).Description;
                jQuery.StateId = db.hState.First(m => m.Description == "Выполнена").StateId;
                jQuery.DateOut = DateTime.Parse(DateTime.Today.ToShortDateString());

                jJournal jJur = new jJournal();
                jJur.Date = DateTime.Now;
                jJur.EventTypeId = db.hEventType.First(m => m.Description == "Смена статуса заявки").EventTypeId;
                jJur.WorkListId = null;
                jJur.PersonId = User.Identity.GetUserId();
                jJur.QueryID = jQuery.QueryId;

                hState newst = db.hState.Find(jQuery.StateId);
                jJur.Description = "c " + oldst + " на " + newst.Description;
                db.jJournal.Add(jJur);
                db.SaveChanges();
            }
        }
    }
}
