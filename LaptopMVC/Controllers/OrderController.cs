using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class OrderController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: Order
        public ActionResult Index()
        {
            //if (Session["ShoppingCart"] != null)
            //{
            //    Session.Remove("ShoppingCart");
            //    return View();
            //}
            //return HttpNotFound();
            return View();
        }
        public ActionResult Confirm(int? id )
        {
            var items = db.tb_order.Find(id);
            if(items == null)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            return View(items);
        }
        public ActionResult Delete(int id)
        {
            var tb_Order = db.tb_order.Find(id);
            var tb_Orderdetails = db.tb_orderdetail.Where(x => x.idorder == id).ToList();
            tb_product tb_Product = new tb_product();
            foreach(var item in tb_Orderdetails)
            {
                tb_Product = db.tb_product.Find(item.idproduct);
                tb_Product.quantity += item.quantity;
                db.tb_orderdetail.Remove(item);
                db.SaveChanges();
            }
            db.tb_order.Remove(tb_Order);
            db.SaveChanges();
            return RedirectToAction("Index", "ShoppingCart");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tb_order tb_Order)
        {
            List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
            if (ModelState.IsValid)
            {
                tb_Order.createdate = DateTime.Now;
                tb_Order.modifieddate = DateTime.Now;
                double sumMoney = ShoppingCart.Sum(x => x.money);
                double price = 0, priceSale = 0;
                Random random = new Random();
                tb_Order.code = "HD" + random.Next(10000, 99999).ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                tb_Order.SumMoney = (decimal)sumMoney;
                tb_Order.typeOrder = 0;
                tb_Order.quantity = ShoppingCart.Sum(x => (x.quantity));
                tb_product tb_Product = new tb_product();
                foreach (var item in ShoppingCart)
                {
                    tb_Order.tb_orderdetail.Add(new tb_orderdetail
                    {
                        idproduct = item.id,
                        quantity = item.quantity,
                    });
                    tb_Product = db.tb_product.Find(item.id);
                    tb_Product.quantity -= item.quantity;
                    price = (double)tb_Product.price;
                    priceSale = (double)tb_Product.pricesale;
                    tb_Product.price = (decimal)price;
                    tb_Product.pricesale = (decimal?)priceSale;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
                db.tb_order.Add(tb_Order);
                db.SaveChanges();
                return RedirectToAction("Confirm", new { id = tb_Order.id });
            }
            return View();
        }
    }
}