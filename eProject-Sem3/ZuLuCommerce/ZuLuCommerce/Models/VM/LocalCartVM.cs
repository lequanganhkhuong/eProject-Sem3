using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZuLuCommerce.Models
{
    public class LocalCartVM
    {
        public int productid { get; set; }
        public string productname { get; set; }
        public string thumbnail { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}