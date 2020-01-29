using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class PurchaseOrder : Document
    {
        public DateTime ExpectedDeliveryDate { get; set; }

        public Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }

        public PurchaseOrder() : base()
        {
        }

        public PurchaseOrder(Staff creator, Supplier supplier, DateTime expectedDeliveryDate) : base(creator)
        {
            Supplier = supplier;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }
    }
}