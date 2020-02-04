using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class InventoryItem 
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Item Code")]
        public string ItemID { get; set; }
        [DisplayName("Store")]
        public int InStoreQty { get; set; }
        [DisplayName("Disbursing")]
        public int InTransitQty { get; set; }
        public int ReorderLvl { get; set; }
        public int ReorderQty { get; set; }
        public int StockCheck { get; set; }

        public Item Item { get; set; }

        // include this 
        //public virtual ICollection<Supplier> Supplier { get; set; }

        public InventoryItem()
        {
        }

        public InventoryItem( int inStoreQty, int inTransitQty, int reorderLvl, int reorderQty, int stockCheck, Item item)
        {
            InStoreQty = inStoreQty;
            InTransitQty = inTransitQty;
            ReorderLvl = reorderLvl;
            ReorderQty = reorderQty;
            StockCheck = stockCheck;
            Item = item;
        }
    }
}