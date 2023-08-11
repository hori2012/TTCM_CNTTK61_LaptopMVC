using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class ContactController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addContact(string name, string email, string website, string message)
        {
            if (ModelState.IsValid)
            {
                tb_contact tb_Contact = new tb_contact();
                tb_Contact.name = name;
                tb_Contact.email = email;
                tb_Contact.website = website;
                tb_Contact.message = message;
                tb_Contact.createdate = DateTime.Now;
                db.tb_contact.Add(tb_Contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}