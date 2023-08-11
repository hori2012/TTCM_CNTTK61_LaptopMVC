using LaptopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private LaptopMVCEntities db = new LaptopMVCEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
            return View(ShoppingCart);
        }
        public RedirectToRouteResult AddToCart(int id)
        {
            if (Session["ShoppingCart"] == null)
            {
                Session["ShoppingCart"] = new List<CartItem>();
            }
            List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
            if (ShoppingCart.FirstOrDefault(x => x.id == id) == null)
            {
                tb_product item = db.tb_product.Find(id);
                var imgCh = item.tb_productImages.FirstOrDefault(x => x.isDefault);
                string img = "";
                if (imgCh != null)
                {
                    img = imgCh.images;
                }
                double price = (double)item.price;
                if (item.pricesale != 0)
                {
                    price = (double)item.pricesale;
                }
                CartItem cartItem = new CartItem()
                {
                    id = item.id,
                    name = item.title,
                    images = img,
                    categories = item.tb_productcategory.title,
                    price = price,
                    quantity = 1
                };
                ShoppingCart.Add(cartItem);
            }
            else
            {
                CartItem cart = ShoppingCart.FirstOrDefault(x => x.id == id);
                cart.quantity++;
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
        public RedirectToRouteResult UpdateQuantity(int id, int newQuantity)
        {
            List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
            CartItem cartItem = ShoppingCart.FirstOrDefault(x => x.id == id);
            tb_product tb_Product = db.tb_product.Find(id);
            if (cartItem != null)
            {
                if (newQuantity <= tb_Product.quantity)
                {
                    cartItem.quantity = newQuantity;
                }
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult RemoveItem(int id)
        {
            List<CartItem> ShoppingCart = (List<CartItem>)Session["ShoppingCart"];
            CartItem cartItem = ShoppingCart.FirstOrDefault(x => x.id == id);
            if (cartItem != null)
            {
                ShoppingCart.Remove(cartItem);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Check()
        {
            return View();
        }
    }
}