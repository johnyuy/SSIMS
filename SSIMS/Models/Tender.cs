using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Tender
    {
        public int TenderID { get; set; }
        public Item TenderItem { get; set; }
        public double Price { get; set; }
    }
}