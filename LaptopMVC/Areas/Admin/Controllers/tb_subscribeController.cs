using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaptopMVC.Models;
using PagedList;

namespace LaptopMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class tb_subscribeController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_subscribe
        public ActionResult Index(int? page, DateTime? date, string searching)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Keywords = searching;
            var items = db.tb_subscribe.OrderBy(n => n.id);
            if (!string.IsNullOrEmpty(searching))
            {
                items = (IOrderedQueryable<tb_subscribe>)items.Where(n => n.email.Contains(searching));
            }
            if(date != null)
            {
                items = (IOrderedQueryable<tb_subscribe>)items.Where(x => DbFunctions.TruncateTime(x.createdate) == DbFunctions.TruncateTime(date));
            }
            else
            {
                date = DateTime.Now;
            }
            ViewBag.date = date;
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/tb_subscribe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_subscribe tb_subscribe = db.tb_subscribe.Find(id);
            if (tb_subscribe == null)
            {
                return HttpNotFound();
            }
            return View(tb_subscribe);
        }

        // GET: Admin/tb_subscribe/Delete/5
        public ActionResult Delete(int? id)
        {
            tb_subscribe tb_Subscribe = db.tb_subscribe.Find(id);
            if(tb_Subscribe == null)
            {
                return HttpNotFound();
            }
            db.tb_subscribe.Remove(tb_Subscribe);
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
