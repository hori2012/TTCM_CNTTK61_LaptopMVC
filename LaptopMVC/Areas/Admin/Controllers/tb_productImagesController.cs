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
    public class tb_productImagesController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_productImages
        public ActionResult Index(int? page, string searching)
        {
            var tb_productImages = db.tb_productImages.Include(t => t.tb_product).OrderBy(t => t.id);
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Keywords = searching;
            if (!string.IsNullOrEmpty(searching))
            {
                tb_productImages = (IOrderedQueryable<tb_productImages>)tb_productImages.Where(n => n.tb_product.title.Contains(searching));
            }
            return View(tb_productImages.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/tb_productImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_productImages tb_productImages = db.tb_productImages.Find(id);
            if (tb_productImages == null)
            {
                return HttpNotFound();
            }
            return View(tb_productImages);
        }

        // GET: Admin/tb_productImages/Create
        public ActionResult Create(int product)
        {
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", product);
            return View();
        }

        // POST: Admin/tb_productImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProduct,images,isDefault")] tb_productImages tb_productImages, HttpPostedFileBase[] images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in images)
                    {
                        if (item != null)
                        {
                            string _fileName = Path.GetFileName(item.FileName);
                            string _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), _fileName);
                            item.SaveAs(_path);
                            tb_productImages.images = _fileName;
                        }
                        tb_productImages.isDefault = false;
                        db.tb_productImages.Add(tb_productImages);
                        db.SaveChanges();
                    }
                    var str = db.tb_product.Find(tb_productImages.idProduct);
                    return RedirectToAction("Index", new {searching = str.title} );
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }

            }

            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_productImages.idProduct);
            return View(tb_productImages);
        }

        // GET: Admin/tb_productImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_productImages tb_productImages = db.tb_productImages.Find(id);
            if (tb_productImages == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_productImages.idProduct);
            return View(tb_productImages);
        }

        // POST: Admin/tb_productImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProduct,images,isDefault")] tb_productImages tb_productImages, HttpPostedFileBase images, FormCollection form)
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
                        tb_productImages.images = _fileName;
                        _path = Path.Combine(Server.MapPath("~/ManagerUploads/"), form["oldimages"]);
                        if (System.IO.File.Exists(_path))
                        {
                            System.IO.File.Delete(_path);
                        }
                    }
                    else
                    {
                        tb_productImages.images = form["oldimages"];
                    }
                    db.Entry(tb_productImages).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.message = "Uploads fail";
                }

            }
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_productImages.idProduct);
            return View(tb_productImages);
        }

        // GET: Admin/tb_productImages/Delete/5
        public ActionResult Delete(int id)
        {
            tb_productImages tb_ProductImages = db.tb_productImages.Find(id);
            if(tb_ProductImages == null)
            {
                return HttpNotFound();
            }
            db.tb_productImages.Remove(tb_ProductImages);
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
