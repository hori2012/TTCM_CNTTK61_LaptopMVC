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
    public class tb_productController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        private List<SelectListItem> categoties = new List<SelectListItem>
        {
            new SelectListItem{Text="Gaming", Value = "Gaming" },
            new SelectListItem{Text="Office", Value = "Office" },
            new SelectListItem{Text="Student", Value = "Student" },
        };
        private List<SelectListItem> cpu = new List<SelectListItem>
            {
                new SelectListItem {Text = "Core i3", Value = "i3"},
                new SelectListItem {Text = "Core i5", Value = "i5"},
                new SelectListItem {Text = "Core i7", Value = "i7"},
                new SelectListItem {Text = "Core i9", Value = "i9"},
                new SelectListItem {Text = "Ryzen 3", Value = "R3"},
                new SelectListItem {Text = "Ryzen 5", Value = "R5"},
                new SelectListItem {Text = "Ryzen 7", Value = "R7"},
                new SelectListItem {Text = "Ryzen 9", Value = "R9"},
            };
        private List<SelectListItem> ram = new List<SelectListItem>
            {
                new SelectListItem {Text = "4gb", Value = "4gb"},
                new SelectListItem {Text = "8gb", Value = "8gb"},
                new SelectListItem {Text = "16gb", Value = "16gb"},
                new SelectListItem {Text = "32gb", Value = "32gb"},
                new SelectListItem {Text = "64gb", Value = "64gb"},
            };
        private List<SelectListItem> screen = new List<SelectListItem>
            {
                new SelectListItem{Text = "Khoảng 13 inch", Value = "13 inch"},
                new SelectListItem{Text = "Khoảng 14 inch", Value = "14 inch"},
                new SelectListItem{Text = "Trên 15 inch", Value = "15 inch" },
            };
        private List<SelectListItem> harddrive = new List<SelectListItem>
            {
                new SelectListItem{Text = "SSD 128gb", Value = "128gb"},
                new SelectListItem{Text = "SSD 256gb", Value = "256gb"},
                new SelectListItem{Text = "SSD 512gb", Value = "512gb"},
                new SelectListItem{Text = "SSD 1tb", Value = "1tb"},
                new SelectListItem{Text = "Trên 1tb", Value = "trên 1tb"},
            };
        private List<SelectListItem> sort = new List<SelectListItem>
        {
            new SelectListItem{Text="Tăng dần", Value="1"},
            new SelectListItem{Text="Giảm dần", Value="0"},
        };
        // GET: Admin/tb_product
        public ActionResult Index(int? page, int? sortProduct, string productCategoties, string searching)
        {
            var tb_product = db.tb_product.Include(t => t.tb_productcategory).OrderBy(x => x.id);
            var tb_orderdetail = db.tb_orderdetail.Include(t => t.tb_product).Include(t => t.tb_order);
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Keywords = searching;
            if (!string.IsNullOrEmpty(searching))
            {
                tb_product = (IOrderedQueryable<tb_product>)tb_product.Where(n => n.title.Contains(searching) || n.description.Contains(searching));
            }
            if (!string.IsNullOrEmpty(productCategoties))
            {
                tb_product = (IOrderedQueryable<tb_product>)tb_product.Where(x => x.tb_productcategory.title.Contains(productCategoties));
            }
            if (sortProduct == null)
            {
                sortProduct = -1;
            }
            else
            {
                if (sortProduct == 1)
                {
                    var result = from od in tb_orderdetail
                                 group od by od.idproduct into g
                                 orderby g.Sum(od => od.quantity)
                                 descending
                                 select new { idproduct = g.Key };
                    var productIds = result.Select(x => x.idproduct).ToList();
                    tb_product = (IOrderedQueryable<tb_product>)productIds.Join(tb_product, id => id, product => product.id, (id, product) => product).AsQueryable();
                }
                else
                {
                    var result = from od in tb_orderdetail
                                 group od by od.idproduct into g
                                 orderby g.Sum(od => od.quantity)
                                 ascending
                                 select new { idproduct = g.Key };
                    var productIds = result.Select(x => x.idproduct).ToList();
                    tb_product = (IOrderedQueryable<tb_product>)productIds.Join(tb_product, id => id, product => product.id, (id, product) => product).AsQueryable();
                }
            }

            ViewBag.productCategoties = new SelectList(categoties, "Value", "Text");
            ViewBag.sortProduct = new SelectList(sort, "Value", "Text");
            return View(tb_product.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/tb_product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_product tb_product = db.tb_product.Find(id);
            if (tb_product == null)
            {
                return HttpNotFound();
            }
            return View(tb_product);
        }

        // GET: Admin/tb_product/Create
        public ActionResult Create()
        {
            ViewBag.idProductCategory = new SelectList(db.tb_productcategory, "id", "title");

            ViewBag.cpu = new SelectList(cpu, "Value", "Text");
            ViewBag.ram = new SelectList(ram, "Value", "Text");
            ViewBag.screen = new SelectList(screen, "Value", "Text");
            ViewBag.harddrive = new SelectList(harddrive, "Value", "Text");
            return View();
        }

        // POST: Admin/tb_product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProductCategory,title,description,detail,cpu,ram,screen,harddrive,price,pricesale,quantity,isSale,isHot,isActive,viewcount")] tb_product tb_product)
        {
            if (ModelState.IsValid)
            {
                tb_product.viewcount = 0;
                if(tb_product.pricesale == 0)
                {
                    tb_product.isSale = false;
                }
                else
                {
                    tb_product.isSale = true;
                }
                db.tb_product.Add(tb_product);
                db.SaveChanges();
                return RedirectToAction("Index", new { searching = tb_product.title });
            }

            ViewBag.idProductCategory = new SelectList(db.tb_productcategory, "id", "title", tb_product.idProductCategory);

            ViewBag.cpu = new SelectList(cpu, "Value", "Text");
            ViewBag.ram = new SelectList(ram, "Value", "Text");
            ViewBag.screen = new SelectList(screen, "Value", "Text");
            ViewBag.harddrive = new SelectList(harddrive, "Value", "Text");
            return View(tb_product);
        }

        // GET: Admin/tb_product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_product tb_product = db.tb_product.Find(id);
            if (tb_product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProductCategory = new SelectList(db.tb_productcategory, "id", "title", tb_product.idProductCategory);

            ViewBag.cpu = new SelectList(cpu, "Value", "Text", tb_product.cpu);
            ViewBag.ram = new SelectList(ram, "Value", "Text", tb_product.ram);
            ViewBag.screen = new SelectList(screen, "Value", "Text", tb_product.screen);
            ViewBag.harddrive = new SelectList(harddrive, "Value", "Text", tb_product.harddrive);
            return View(tb_product);
        }

        // POST: Admin/tb_product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProductCategory,title,description,detail,cpu,ram,screen,harddrive,price,pricesale,quantity,isSale,isHot,isActive,viewcount")] tb_product tb_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProductCategory = new SelectList(db.tb_productcategory, "id", "title", tb_product.idProductCategory);
            ViewBag.cpu = new SelectList(cpu, "Value", "Text", tb_product.cpu);
            ViewBag.ram = new SelectList(ram, "Value", "Text", tb_product.ram);
            ViewBag.screen = new SelectList(screen, "Value", "Text", tb_product.screen);
            ViewBag.harddrive = new SelectList(harddrive, "Value", "Text", tb_product.harddrive);
            return View(tb_product);
        }

        // GET: Admin/tb_product/Delete/5
        public ActionResult Delete(int id)
        {
            tb_product tb_Product = db.tb_product.Find(id);
            if (tb_Product == null)
            {
                return HttpNotFound();
            }
            db.tb_product.Remove(tb_Product);
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
