using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Tender
    {
        public int TenderID { get; set; }
        public int ItemID { get; set; }
        public int SupplierID { get; set; }
        public double Price { get; set; }

        public Item Item { get; set; }
        public Supplier Supplier { get; set; }

        public Tender()
        {
        }

        public Tender(int itemID, int supplierID, double price)
        {
            ItemID = itemID;
            SupplierID = supplierID;
            Price = price;
        }
    }

}