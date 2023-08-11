using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaptopMVC.Models;
using PagedList;

namespace LaptopMVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class tb_orderController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        List<SelectListItem> type = new List<SelectListItem>
            {
            new SelectListItem{Text="------Select type order------", Value="-1"},
                new SelectListItem{Text = "Chờ thanh toán", Value = "0" },
                new SelectListItem{Text = "Đã thanh toán", Value = "1"},
                new SelectListItem{Text = "Hủy thanh toán", Value = "2"},
                new SelectListItem{Text = "Đang giao hàng", Value = "3"},
            };

        // GET: Admin/tb_order
        public ActionResult Index(int? page, int? typeOrder, DateTime? date, string searching)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var items = db.tb_order.OrderBy(x => x.id);
            ViewBag.Keywords = searching;
            if (!String.IsNullOrEmpty(searching))
            {
                items = (IOrderedQueryable<tb_order>)items.Where(x => x.customername.Contains(searching));
            }
            if (typeOrder != null && typeOrder != -1)
            {
                items = (IOrderedQueryable<tb_order>)items.Where(x => x.typeOrder == typeOrder);
            }
            if (date == null)
            {
                date = DateTime.Now;
            }
            else
            {
                items = (IOrderedQueryable<tb_order>)items.Where(x => DbFunctions.TruncateTime(x.createdate) == DbFunctions.TruncateTime(date));
            }
            ViewBag.date = date;
            ViewBag.type = new SelectList(type, "Value", "Text");
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/tb_order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_order tb_order = db.tb_order.Find(id);
            if (tb_order == null)
            {
                return HttpNotFound();
            }
            return View(tb_order);
        }

        // GET: Admin/tb_order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_order tb_order = db.tb_order.Find(id);
            ViewBag.date = tb_order.createdate;
            string code = tb_order.code;
            ViewBag.code = code;
            ViewBag.typeOrder = new SelectList(type, "Value", "Text");
            if (tb_order == null)
            {
                return HttpNotFound();
            }
            return View(tb_order);
        }

        // POST: Admin/tb_order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,code,customername,phone,Address,email,SumMoney,quantity,typeOrder,createdate")] tb_order tb_order)
        {
            if (ModelState.IsValid)
            {
                tb_order.modifieddate = DateTime.Now;
                db.Entry(tb_order).State = EntityState.Modified;
                if (tb_order.typeOrder == 2)
                {
                    var model = db.tb_orderdetail.Where(x => x.tb_order.id == tb_order.id).ToList();

                    tb_product tb_Product = new tb_product();
                    foreach (var item in model)
                    {
                        tb_Product = db.tb_product.Find(item.idproduct);
                        tb_Product.quantity += item.quantity;
                        double price = (double)tb_Product.price;
                        double priceSale = (double)tb_Product.pricesale;
                        tb_Product.price = (decimal)price;
                        tb_Product.pricesale = (decimal?)priceSale;
                    }
                }
                db.SaveChanges();
                ViewBag.typeOrder = new SelectList(type, "Value", "Text");
                return RedirectToAction("Index");
            }
            return View(tb_order);
        }

        // GET: Admin/tb_order/Delete/5
        public ActionResult Delete(int id)
        {
            tb_order tb_Order = db.tb_order.Find(id);
            if (tb_Order == null)
            {
                return HttpNotFound();
            }
            db.tb_order.Remove(tb_Order);
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
