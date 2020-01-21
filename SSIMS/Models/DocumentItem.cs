using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DocumentItem
    {
        public int DocumentItemID {get; set;}
        public int ItemID { get; set; }
        public int DocumentID { get; set; }
        public int Qty { get; set; }

        public Item Item { get; set; }
        public Document Document { get; set; }

        public DocumentItem()
        {
        }

        public DocumentItem(int itemID, int documentID, int qty, Item item, Document document)
        {
            ItemID = itemID;
            DocumentID = documentID;
            Qty = qty;
            Item = item;
            Document = document;
        }
    }
}