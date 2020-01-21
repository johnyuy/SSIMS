using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class PurchaseOrder : Document
    {
        public int SupplierID { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }

        public Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }

        public PurchaseOrder(int creatorID, int responderID, DateTime createdDate, DateTime responseDate, Status status, int supplierID, DateTime expectedDeliveryDate) : base(creatorID, responderID, createdDate, responseDate, status)
        {
            SupplierID = supplierID;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }
    }
}