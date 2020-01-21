using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class PurchaseItem
    {
        public int PurchaseItemID { get; set; }
        public int TenderID { get; set; }
        public int Qty { get; set; }
 
        public Tender Tender { get; set; }

        public PurchaseItem()
        {
        }

        public PurchaseItem(int purchaseItemId, int tenderID, int qty)
        {
            PurchaseItemID = purchaseItemId;
            TenderID = tenderID;
            Qty = qty;
        }
    }
}