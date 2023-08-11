using LaptopMVC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class ShopController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
      
        // GET: Shop
        public ActionResult Index(int? page, string searching, string categories)
        {
            page = page ?? 1;
            var items = db.tb_product.Where(x => x.isActive).OrderBy(x => x.id);
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            if (String.IsNullOrEmpty(categories))
            {
                categories = "Shop";
            }
            if (!categories.Equals("Shop"))
            {
                items = (IOrderedQueryable<tb_product>)items.Where(x => x.tb_productcategory.title.Contains(categories));
            }
            if (!String.IsNullOrEmpty(searching))
            {
                items = (IOrderedQueryable<tb_product>)items.Where(x => x.title.Contains(searching));
            }
            if(items.Count() != 0)
            {
                pageSize = items.Count();
            }
            ViewBag.Categories = categories;
            ViewBag.Keywords = searching;
            return View(items.ToPagedList(pageNumber, pageSize));
        }
    }
}