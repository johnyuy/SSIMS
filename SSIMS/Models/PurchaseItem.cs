using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class PurchaseItem
    {
        public int PurchaseItemId { get; set; }
        public int TenderID { get; set; }
        public int Qty { get; set; }
 
        public Tender Tender { get; set; }

        public PurchaseItem()
        {
        }

        public PurchaseItem(int purchaseItemId, int tenderID, int qty)
        {
            PurchaseItemId = purchaseItemId;
            TenderID = tenderID;
            Qty = qty;
        }
    }
}