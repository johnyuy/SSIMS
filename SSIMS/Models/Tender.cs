using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Tender
    {
        public int ID { get; set; }
        public virtual Item Item { get; set; }
        public virtual Supplier Supplier { get; set; }
        public double Price { get; set; }

        public Tender()
        {
        }

        public Tender(Item item, Supplier supplier, double price)
        {
            Item = item;
            Supplier = supplier;
            Price = price;
        }
    }

}