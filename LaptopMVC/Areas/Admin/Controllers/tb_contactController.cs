using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaptopMVC.Models;

namespace LaptopMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class tb_contactController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_contact
        public ActionResult Index(DateTime? date, string searching)
        {
            var items = db.tb_contact.OrderBy(x => x.id);
            if (!string.IsNullOrEmpty(searching))
            {
                items = (IOrderedQueryable<tb_contact>)items.Where(x => x.name.Equals(searching) || x.email.Equals(searching));
            }
            if(date != null)
            {
                items = (IOrderedQueryable<tb_contact>)items.Where(x=>DbFunctions.TruncateTime(x.createdate) == DbFunctions.TruncateTime(date));
            }
            else
            {
                date = DateTime.Now;
            }
            ViewBag.Keywords = searching;
            ViewBag.date = date;
            return View(items.ToList());
        }

        // GET: Admin/tb_contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_contact tb_contact = db.tb_contact.Find(id);
            tb_contact.isRead = true;
            db.SaveChanges();
            if (tb_contact == null)
            {
                return HttpNotFound();
            }
            return View(tb_contact);
        }

        // GET: Admin/tb_contact/Delete/5
        public ActionResult Delete(int id)
        {
            tb_contact tb_Contact = db.tb_contact.Find(id);
            if(tb_Contact == null)
            {
                return HttpNotFound();
            }
            db.tb_contact.Remove(tb_Contact);
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
