using System;
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
    //Контроллер отвечает за обработку входящих запросов, 
    //выполнение операций над моделью предметной области и выбор представлений для визуализации пользователю.
    public class jQueriesController : Controller
    {
        //Создаем экземпляр класса контекста для взаимодействия с нашей бд 
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();      
        // GET: jQueries
        // Метод Index, в нем мы задаем связи с какими таблицами нам будут нужны для оботражения данных в представлени
        //Результатом работы метода является вызов представления Index
        public ActionResult Index()
        {
            var jQuery = db.jQuery.Include(j => j.AspNetUsers).Include(j => j.AspNetUsers1).Include(j => j.hState);
            return View(jQuery.ToList());
        }

        // GET: jQueries/Details/5
        // Метод Детали, имеет аргумент id, чтобы получать из базы данные только по 1 конкретной заявке
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Производим поиск по id
            jQuery jQuery = db.jQuery.Find(id);
            if (jQuery == null)
            {
                return HttpNotFound();
            }
            return View(jQuery);
        }

        // GET: jQueries/Create
        // Используем ViewBag для того чтобы в представлении у нас были вместо вторичных ключей конкретные значения из связаных таблиц
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio");
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio");
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description");
            return View();
        }

        // POST: jQueries/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //Обрабатываем данные полученые из представления
        [ValidateAntiForgeryToken] 
        public ActionResult Create([Bind(Include = "QueryId,DateOut,DateIn,DateModification,DeadLine,Text,StateId,PersonId,PersonSpId")] jQuery jQuery)
        {
            if (ModelState.IsValid)
            {
                //Заполняем необходимые для для создания заявки поля, которые не видит пользователь 
                jQuery.DateIn = DateTime.Parse(DateTime.Today.ToShortDateString());
                jQuery.DateModification = DateTime.Now;
                jQuery.DateModification = jQuery.DateModification.AddMilliseconds(-jQuery.DateModification.Millisecond);
                DateTime dmd = jQuery.DateModification;             
                jQuery.StateId = db.hState.First(m => m.Description == "Ожидает").StateId;
                jQuery.PersonId = User.Identity.GetUserId();
                db.jQuery.Add(jQuery);
                db.SaveChanges();

                jJournal jJur = new jJournal();
                string dmdstr = dmd.ToString("yyyy-MM-dd HH:mm:ss") + ".000";
                DateTime dmdn = DateTime.Parse(dmdstr);
                db = new KOMK_Main_v2Entities();
                jQuery jQ = db.jQuery.First(m => m.DateModification== dmdn);    

                jJur.Date= DateTime.Now;
                jJur.EventTypeId = db.hEventType.First(m => m.Description == "Создание заявки").EventTypeId; 
                jJur.WorkListId = null;                
                jJur.PersonId = User.Identity.GetUserId();
                jJur.QueryID = jQ.QueryId;
                jJur.Description = " Содержание:  "+jQ.Text;


                db.jJournal.Add(jJur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Если данные не валидны то делаем тоже что и в методе GET
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonId);
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonSpId);
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description", jQuery.StateId);
            return View(jQuery);
        }



        // GET: jQueries/ChangeStatusQuery/5
        // Метод Смена статуса заявки, имеет аргумент id, чтобы производить действия над конкретной заявкой
        public ActionResult ChangeStatusQuery(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jQuery jQuery = db.jQuery.Find(id);
            TempData["oldState"] = jQuery.StateId;
            if (jQuery == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonId);
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonSpId);
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description", jQuery.StateId);
            return View(jQuery);
        }

        // POST: jQueries/ChangeStatusQuery/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // Сохраняем изменения 
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatusQuery([Bind(Include = "QueryId,DateOut,DateIn,DateModification,DeadLine,Text,StateId,PersonId,PersonSpId")] jQuery jQuery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jQuery).State = EntityState.Modified;
                if (jQuery.StateId == db.hState.First(m => m.Description == "Выполнена").StateId || jQuery.StateId == db.hState.First(m => m.Description == "Отклонена").StateId)
                {
                    jQuery.DateOut= DateTime.Parse(DateTime.Today.ToShortDateString());
                }
                if (Convert.ToInt32(TempData["oldState"])!= jQuery.StateId) {
                    jJournal jJur = new jJournal();
                    jJur.Date = DateTime.Now;
                    jJur.EventTypeId = db.hEventType.First(m => m.Description == "Смена статуса заявки").EventTypeId;
                    jJur.WorkListId = null;
                    jJur.PersonId = User.Identity.GetUserId();
                    jJur.QueryID = jQuery.QueryId;
                    hState oldst = db.hState.Find(Convert.ToInt32(TempData["oldState"]));
                    hState newst = db.hState.Find(jQuery.StateId);
                    jJur.Description = "c " + oldst.Description + " на " + newst.Description;
                    db.jJournal.Add(jJur);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonId);
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonSpId);
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description", jQuery.StateId);
            return View(jQuery);
        }

        // GET: jQueries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jQuery jQuery = db.jQuery.Find(id);
            if (jQuery == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonId);
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonSpId);
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description", jQuery.StateId);
            return View(jQuery);
        }

        // POST: jQueries/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QueryId,DateOut,DateIn,DateModification,DeadLine,Text,StateId,PersonId,PersonSpId")] jQuery jQuery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jQuery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonId);
            ViewBag.PersonSpId = new SelectList(db.AspNetUsers, "Id", "Fio", jQuery.PersonSpId);
            ViewBag.StateId = new SelectList(db.hState, "StateId", "Description", jQuery.StateId);
            return View(jQuery);
        }
        // GET: jQueries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            jQuery jQuery = db.jQuery.Find(id);
            if (jQuery == null)
            {
                return HttpNotFound();
            }
            return View(jQuery);
        }

        // POST: jQueries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            jQuery jQuery = db.jQuery.Find(id);
            db.jQuery.Remove(jQuery);
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
