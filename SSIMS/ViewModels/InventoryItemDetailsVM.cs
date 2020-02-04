using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Service;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.ViewModels
{
    public class InventoryItemDetailsVM
    {
        [Display(Name="Item Code")]
        public string ItemCode;
        [Display(Name ="Category")]
        public string Category;
        [Display(Name ="Description")]
        public string Description;
        [Display(Name = "Unit of Measure")]
        public string UOM;
        [Display(Name = "In Store Quantity")]
        public int InStoreQty;
        [Display(Name = "Low Stock Level")]
        public int ReorderLvl;
        [Display(Name = "Reorder Quantity")]
        public int ReorderQty;
        [Display(Name = "Tender Price")]
        public string TenderPrice;

        public string Supplier1;

        public string Supplier2;

        public string Supplier3;

        public string LastOrder;

        public string ItemNumber;

        public string ImageURL;

        public InventoryItemDetailsVM(InventoryItem item)
        {
            ItemCode = item.ItemID;
            Category = item.Item.Category;
            Description = item.Item.Description;
            UOM = item.Item.UnitOfMeasure;
            InStoreQty = item.InStoreQty;
            ReorderLvl = item.ReorderLvl;
            ReorderQty = item.ReorderQty;
            ImageURL = item.Item.ImageURL;
            //get top 3 tenders using TenderService

            //get last Purchase using PurchaseService
        }

    }
}