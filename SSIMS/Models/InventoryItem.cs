using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class InventoryItem : Item
    {
        public string InventoryItemID { get; set; }
        public int InStoreQty { get; set; }
        public int InTransitQty { get; set; }
        public int ReorderLvl { get; set; }
        public int ReorderQty { get; set; }
        public int StockCheck { get; set; }

        public ICollection<Tender> Tenders { get; set; }

        public InventoryItem()
        {
        }

        public InventoryItem(string inventoryItemID, int inStoreQty, int inTransitQty, int reorderLvl, int reorderQty, int stockCheck)
        {
            InventoryItemID = inventoryItemID;
            InStoreQty = inStoreQty;
            InTransitQty = inTransitQty;
            ReorderLvl = reorderLvl;
            ReorderQty = reorderQty;
            StockCheck = stockCheck;
            ItemTenders = new Tender[2];
        }
    }
}