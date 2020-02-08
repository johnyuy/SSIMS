using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class InventoryCheckVM
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Display(Name = "Unit of Measure")]
        public string UOM { get; set; }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        
        public string ImageURL { get; set; }

        public InventoryCheckVM() {}
        //construct using inventory item
        public InventoryCheckVM(InventoryItem item)
        {
            ItemCode = item.Item.ID;
            Category = item.Item.Category;
            Description = item.Item.Description;
            UOM = item.Item.UnitOfMeasure;
            Qty = item.InStoreQty;
            ImageURL = item.Item.ImageURL;
        }

    }


    
}