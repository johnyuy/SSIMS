using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class ItemTransaction
    {
        public int TransactionID;
        public int ItemID;
        public int HandOverQty;
        public int TakeOverQty;
        public string Reason;

        public Item Item;

        public ItemTransaction()
        {
        }

        public ItemTransaction(int handOverQty, int takeOverQty, string reason, int itemID)
        {
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
            ItemID = itemID;
        }
    }
}