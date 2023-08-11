using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaptopMVC.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string images { get; set; }
        public string categories { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double money
        {
            get
            {
                return (price * quantity);
            }
        }
    }
}