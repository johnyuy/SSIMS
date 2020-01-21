using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class StockCardEntry
    {
        public int StockCardEntryId { get; set; }
        public int ItemID { get; set; }
        public int DocumentID { get; set; }
        public int Qty { get; set; }
        public int Balance { get; set; }

        public Item Item { get; set; }

        public Document Document { get; set; }

        public StockCardEntry(int itemID, int documentID, int qty, int balance)
        {
            ItemID = itemID;
            DocumentID = documentID;
            Qty = qty;
            Balance = balance;
        }

        public StockCardEntry()
        {
        }
    }
}