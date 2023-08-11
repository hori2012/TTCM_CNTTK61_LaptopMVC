using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class HomeController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        public ActionResult Index()
        {
            var items = db.tb_product.Where(x => x.isActive).OrderBy(keySelector: x => Guid.NewGuid()).Take(10).ToList();
            return View(items);
        }
        public PartialViewResult Banner()
        {
            return PartialView("_Banner", db.tb_productcategory.ToList());
        }
        public PartialViewResult New()
        {
            var items = db.tb_news.Where(x => x.isActive).Take(3).ToList();
            return PartialView("_News", items);
        }
        public PartialViewResult ProductHot()
        {
            return PartialView("_ProductHot", db.tb_product.Where(x=>x.isActive && x.isHot).ToList());
        }
        public ActionResult addSubscribe(string email)
        {
            tb_subscribe tb_Subscribe = new tb_subscribe();
            tb_Subscribe.email = email;
            tb_Subscribe.createdate = DateTime.Now;
            db.tb_subscribe.Add(tb_Subscribe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult _Cart()
        {
            int sumItem = 0;
            if (Session["ShoppingCart"] != null)
            {
                List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
                sumItem = ShoppingCart.Sum(x => x.quantity);
            }
            ViewBag.sumItem = sumItem;
            return PartialView();
        }
    }
}