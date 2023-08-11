using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class CustomerController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: Customer
        public ActionResult Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return HttpNotFound();
            }

            var items = db.tb_order.Where(x=>x.email == email).ToList();
            return View(items);
        }
        public ActionResult Details(int id)
        {
            tb_order tb_Order = db.tb_order.Find(id);
            return View(tb_Order);
        }

        public RedirectToRouteResult UnOrder(int id)
        {
            tb_order tb_Order = db.tb_order.Find(id);
            var tb_Orderdetails = db.tb_orderdetail.Where(x => x.idorder == id).ToList();
            tb_product tb_Product = new tb_product();
            foreach (var item in tb_Orderdetails)
            {
                tb_Product = db.tb_product.Find(item.idproduct);
                tb_Product.quantity += item.quantity;
                db.SaveChanges();
            }
            tb_Order.typeOrder = 2;
            tb_Order.modifieddate= DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index", new {email = tb_Order.email});
        }
        public RedirectToRouteResult Recevied(int id)
        {
            tb_order tb_Order = db.tb_order.Find(id);
            tb_Order.typeOrder = 1;
            db.SaveChanges();

            return RedirectToAction("Index", new { email = User.Identity.Name });
        }
    }
}