using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaptopMVC.Models;
using PagedList;

namespace LaptopMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class tb_newsController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_news
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Keywords = searching;
            var news = db.tb_news.OrderBy(n => n.id);
            if (!string.IsNullOrEmpty(searching))
            {
                news = (IOrderedQueryable<tb_news>)news.Where(n => n.title.Contains(searching) || n.description.Contains(searching));
            }
            return View(news.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/tb_news/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_news tb_news = db.tb_news.Find(id);
            if (tb_news == null)
            {
                return HttpNotFound();
            }
            return View(tb_news);
        }

        // GET: Admin/tb_news/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/tb_news/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,description,detail,images,createdate,modifieddate,isActive")] tb_news tb_news, HttpPostedFileBase images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tb_news.createdate = DateTime.Now;
                    tb_news.modifieddate = DateTime.Now;
                    if (images != null)
                    {
                        string _fileName = Path.GetFileName(images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), _fileName);
                        images.SaveAs(_path);
                        tb_news.images = _fileName;
                    }
                    db.tb_news.Add(tb_news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }
               
            }

            return View(tb_news);
        }

        // GET: Admin/tb_news/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_news tb_news = db.tb_news.Find(id);
            ViewBag.date = tb_news.createdate;
            if (tb_news == null)
            {
                return HttpNotFound();
            }
            return View(tb_news);
        }

        // POST: Admin/tb_news/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,description,detail,images,createdate,modifieddate,isActive")] tb_news tb_news, HttpPostedFileBase images, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tb_news.modifieddate = DateTime.Now;
                    if (images != null)
                    {
                        string _fileName = Path.GetFileName(images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), _fileName);
                        images.SaveAs(_path);
                        tb_news.images = _fileName;
                        _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), form["oldimages"]);
                        if (System.IO.File.Exists(_path))
                        {
                            System.IO.File.Delete(_path);
                        }
                    }
                    else
                    {
                        tb_news.images = form["oldimages"];
                    }
                    db.Entry(tb_news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }
               
            }
            return View(tb_news);
        }

        // GET: Admin/tb_news/Delete/5
        public ActionResult Delete(int id)
        {
            tb_news tb_News = db.tb_news.Find(id);
            if(tb_News == null)
            {
                return HttpNotFound();
            }
            db.tb_news.Remove(tb_News);
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
