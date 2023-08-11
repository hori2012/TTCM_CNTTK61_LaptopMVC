using LaptopMVC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class NewController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: New
        public ActionResult Index(int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var items = db.tb_news.Where(x => x.isActive).OrderBy(x => x.id);
            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var items = db.tb_news.Find(id);
            return View(items);
        }
    }
}