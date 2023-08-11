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
    public class tb_reviewController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_review
        public ActionResult Index(DateTime? date, string searching)
        {
            var tb_review = db.tb_review.Include(t => t.tb_product);
            if (!string.IsNullOrEmpty(searching))
            {
                tb_review = tb_review.Where(x=>x.name.Equals(searching) || x.email.Equals(searching));
            }
            if(date != null)
            {
                tb_review = tb_review.Where(x=>DbFunctions.TruncateTime(x.createdate) == DbFunctions.TruncateTime(date));
            }
            else
            {
                date = DateTime.Now;
            }
            ViewBag.Keywords = searching;
            ViewBag.date = date;
            return View(tb_review.ToList());
        }

        // GET: Admin/tb_review/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_review tb_review = db.tb_review.Find(id);
            if (tb_review == null)
            {
                return HttpNotFound();
            }
            return View(tb_review);
        }

        // GET: Admin/tb_review/Create
        public ActionResult Create()
        {
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title");
            return View();
        }

        // POST: Admin/tb_review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProduct,name,email,review,createdate")] tb_review tb_review)
        {
            if (ModelState.IsValid)
            {
                db.tb_review.Add(tb_review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_review.idProduct);
            return View(tb_review);
        }

        // GET: Admin/tb_review/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_review tb_review = db.tb_review.Find(id);
            if (tb_review == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_review.idProduct);
            return View(tb_review);
        }

        // POST: Admin/tb_review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProduct,name,email,review,createdate")] tb_review tb_review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduct = new SelectList(db.tb_product, "id", "title", tb_review.idProduct);
            return View(tb_review);
        }

        // GET: Admin/tb_review/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_review tb_review = db.tb_review.Find(id);
            if (tb_review == null)
            {
                return HttpNotFound();
            }
            return View(tb_review);
        }

        // POST: Admin/tb_review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_review tb_review = db.tb_review.Find(id);
            db.tb_review.Remove(tb_review);
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
