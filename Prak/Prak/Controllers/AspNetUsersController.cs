using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using Prak.Models;
using Calabonga.Xml.Exports;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;
using System.Globalization;


namespace Prak.Controllers
{
    public class AspNetUsersController : Controller
    {
        private KOMK_Main_v2Entities db = new KOMK_Main_v2Entities();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Fio")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Fio")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
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
        public List<AspNetUsers> GetUsers()
        {
            List<AspNetUsers> users = db.AspNetUsers.ToList();
            return users;
        }
        public ActionResult Export()
        {
            string result = string.Empty;
            Workbook wb = new Workbook();

            // properties
            wb.Properties.Author = "Calabonga";
            wb.Properties.Created = DateTime.Today;
            wb.Properties.LastAutor = "Calabonga";
            wb.Properties.Version = "14";

            // options sheets
            wb.ExcelWorkbook.ActiveSheet = 1;
            wb.ExcelWorkbook.DisplayInkNotes = false;
            wb.ExcelWorkbook.FirstVisibleSheet = 1;
            wb.ExcelWorkbook.ProtectStructure = false;
            wb.ExcelWorkbook.WindowHeight = 800;
            wb.ExcelWorkbook.WindowTopX = 0;
            wb.ExcelWorkbook.WindowTopY = 0;
            wb.ExcelWorkbook.WindowWidth = 600;

            // create style s1 for header
            //Style s1 = new Style("s1");
            //s1.Font.Bold = true;
            //s1.Font.Italic = true;
            //s1.Font.Color = "#FF0000";
            //wb.AddStyle(s1);

            //// create style s2 for header
            //Style s2 = new Style("s2");
            //s2.Font.Bold = true;
            //s2.Font.Italic = true;
            //s2.Font.Size = 12;
            //s2.Borders.Add(new Border());
            //s2.Font.Color = "#0000FF";
            //wb.AddStyle(s2);
            //// Third sheet 
            //

            //// Adding Headers


            // get data
            List<AspNetUsers> people = GetUsers();
            Worksheet ws3 = new Worksheet("Пользователи");
            ws3.AddCell(0, 0, "Фио");
            ws3.AddCell(0, 1, "Пароль");
            ws3.AddCell(0, 2, "Соль");
            ws3.AddCell(0, 3, "Почта");
            ws3.AddCell(0, 4, "UserName");
            int totalRows = 0;

            // appending rows with data
            for (int i = 0; i < people.Count; i++)
            {
                ws3.AddCell(i + 1, 0, people[i].Fio);
                ws3.AddCell(i + 1, 1, people[i].PasswordHash);
                ws3.AddCell(i + 1, 2, people[i].SecurityStamp);
                ws3.AddCell(i + 1, 3, people[i].Email);
                ws3.AddCell(i + 1, 4, people[i].UserName);
                totalRows++;
            }



            wb.AddWorksheet(ws3);

            // generate xml 
            string workstring = wb.ExportToXML();

            // Send to user file
            return new ExcelResult("Persons.xls", workstring);
        }
        public class ExcelResult : ActionResult
            {
                /// <summary>
                /// Создает экземпляр класса, которые выдает файл Excel
                /// </summary>
                /// <param name="fileName">наименование файла для экспорта</param>
                /// <param name="report">готовый набор данные для экпорта</param>
                public ExcelResult(string fileName, string report)
                {
                    this.Filename = fileName;
                    this.Report = report;
                }
                public string Report { get; private set; }
                public string Filename { get; private set; }

                public override void ExecuteResult(ControllerContext context)
                {
                var ctx1 = context.HttpContext;
                ctx1.Response.Clear();
                ctx1.Response.ContentType = "application/vnd.ms-excel";
                ctx1.Response.BufferOutput = true;
                ctx1.Response.AddHeader("content-disposition",
                                         string.Format("attachment; filename={0}", Filename));
                ctx1.Response.ContentEncoding = Encoding.UTF8;
                ctx1.Response.Charset = "utf-8";
                ctx1.Response.Write(Report);
                ctx1.Response.Flush();
                ctx1.Response.End();
                }
            }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Файл не выбран! <br>";
                return View("Index", db.AspNetUsers.ToList());
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    db = new KOMK_Main_v2Entities();
                    string path = Server.MapPath("~/Import/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    //Читаем из файла
                   // Excel.Application ap = new Excel.Application();

                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<AspNetUsers> listUsers = new List<AspNetUsers>();
                    for(int row = 2; row <= range.Rows.Count; row++)
                    {
                        AspNetUsers user = new AspNetUsers();
                        user.Fio = ((Excel.Range)range.Cells[row, 1]).Text;
                        user.PasswordHash = ((Excel.Range)range.Cells[row, 2]).Text;
                        user.SecurityStamp = ((Excel.Range)range.Cells[row, 3]).Text;
                        user.Email = ((Excel.Range)range.Cells[row, 4]).Text;
                        user.UserName = ((Excel.Range)range.Cells[row, 5]).Text;
                        user.Id = Guid.NewGuid().ToString();
                        user.EmailConfirmed = false;
                        user.PhoneNumberConfirmed = false;
                        user.TwoFactorEnabled = false;
                        user.LockoutEnabled = true;
                        user.AccessFailedCount = 0;
                        db.AspNetUsers.Add(user);
                        db.SaveChanges();
                        //AspNetUserRoles acc = new AspNetUserRoles();
                        //acc.UserId = user.Id;
                        //acc.RoleId = db.AspNetRoles.Where(m => m.Name == "User").First().Id;
                        
                        //db.AspNetUserRoles.Add(acc);
                        //db.SaveChanges();

                    }
                    workbook.Close();
                    ViewBag.Error = "Данные загружены <br>";
                    return View("Index", db.AspNetUsers.ToList());
                }
                else
                {
                    ViewBag.Error = "Это не Excel! <br>";
                    return View("Index", db.AspNetUsers.ToList());
                }
            }
            
        }

        }
}
