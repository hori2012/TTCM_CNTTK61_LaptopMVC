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
    public class tb_orderdetailController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();

        // GET: Admin/tb_orderdetail
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var tb_orderdetail = db.tb_orderdetail.Where(x=>x.idorder == id);
            return View(tb_orderdetail.ToList());
        }
    }
}
