using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class ItemTransaction
    {
        public int TransactionID;
        public int HandOverQty;
        public int TakeOverQty;
        public string Reason;
        public Item Item;

        public ItemTransaction()
        {
        }

        public ItemTransaction(int transactionID, int handOverQty, int takeOverQty, string reason, Item item)
        {
            TransactionID = transactionID;
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
            Item = item;
        }
    }
}