using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SSIMS.Models
{
    public class TransactionItem
    {
        public int TransactionItemID { get; set; }
        public int ItemID { get; set; }
        public int HandOverQty { get; set; }
        public int TakeOverQty { get; set; }
        public string Reason { get; set; }

        public Item Item { get; set; }

        public TransactionItem()
        {
        }

        public TransactionItem(int handOverQty, int takeOverQty, string reason, int itemID)
        {
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
            ItemID = itemID;
        }
    }
}