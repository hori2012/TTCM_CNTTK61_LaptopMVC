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

namespace LaptopMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class tb_productcategoryController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_productcategory
        public ActionResult Index(string searching)
        {
            var items = db.tb_productcategory.OrderBy(x => x.id);
            ViewBag.Keywords = searching;
            if (!String.IsNullOrEmpty(searching))
            {
                items = (IOrderedQueryable<tb_productcategory>)items.Where(x => x.title.Contains(searching));
            }
            return View(items.ToList());
        }

        // GET: Admin/tb_productcategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_productcategory tb_productcategory = db.tb_productcategory.Find(id);
            if (tb_productcategory == null)
            {
                return HttpNotFound();
            }
            return View(tb_productcategory);
        }

        // GET: Admin/tb_productcategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/tb_productcategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,images,createdate")] tb_productcategory tb_productcategory, HttpPostedFileBase images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tb_productcategory.createdate = DateTime.Now;
                    if (images != null)
                    {
                        string _fileName = Path.GetFileName(images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), _fileName);
                        images.SaveAs(_path);
                        tb_productcategory.images = _fileName;
                    }
                    tb_productcategory.createdate = DateTime.Now;
                    db.tb_productcategory.Add(tb_productcategory);
                    db.SaveChanges();
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }
                
                return RedirectToAction("Index");
            }

            return View(tb_productcategory);
        }

        // GET: Admin/tb_productcategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_productcategory tb_productcategory = db.tb_productcategory.Find(id);
            ViewBag.date = tb_productcategory.createdate;
            if (tb_productcategory == null)
            {
                return HttpNotFound();
            }
            return View(tb_productcategory);
        }

        // POST: Admin/tb_productcategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,images,createdate")] tb_productcategory tb_productcategory, HttpPostedFileBase images, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (images != null)
                    {
                        string _fileName = Path.GetFileName(images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), _fileName);
                        images.SaveAs(_path);
                        tb_productcategory.images = _fileName;
                        _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), form["oldimages"]);
                        if (System.IO.File.Exists(_path))
                        {
                            System.IO.File.Delete(_path);
                        }
                    }
                    else
                    {
                        tb_productcategory.images = form["oldimages"];
                    }
                    db.Entry(tb_productcategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }
                
            }
            return View(tb_productcategory);
        }

       public ActionResult Delete(int id)
        {
            tb_productcategory tb_Productcategory = db.tb_productcategory.Find(id);
            if(tb_Productcategory == null)
            {
                return HttpNotFound();
            }
            db.tb_productcategory.Remove(tb_Productcategory);
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
