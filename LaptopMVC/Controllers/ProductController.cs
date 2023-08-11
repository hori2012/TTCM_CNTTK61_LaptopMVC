using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class ProductController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: Product
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var items = db.tb_product.Find(id);
            items.viewcount += 1;
            double price = (double)items.price;
            double priceSale = (double)items.pricesale;
            items.price = (decimal)price;
            items.pricesale = (decimal?)priceSale;
            db.SaveChanges();
            return View(items);
        }
        public ActionResult AddReview(int id, string name, string email, string message)
        {
            tb_review  tb_Review = new tb_review();
            tb_Review.createdate = DateTime.Now;
            tb_Review.name = name; 
            tb_Review.email = email;
            tb_Review.review = message;
            tb_Review.idProduct = id;
            db.tb_review.Add(tb_Review);
            db.SaveChanges();
            return RedirectToAction("Index", new {id = id});
        }
    }
}