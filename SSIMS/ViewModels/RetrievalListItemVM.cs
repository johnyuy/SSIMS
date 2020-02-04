using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class RetrievalListItemVM
    {
        public string ItemID;
        public int HandoverQty;
        public int TakeoverQty;
        public string Reason = "retrieval";

        public RetrievalListItemVM(string itemID, int qty)
        {
            ItemID = itemID;
            HandoverQty = qty;
            TakeoverQty = HandoverQty;
        }
    }
}