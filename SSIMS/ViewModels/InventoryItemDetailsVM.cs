using System;
using System.Linq;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Service;
using System.ComponentModel;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.ViewModels
{
    public class InventoryItemDetailsVM
    {
        
        [Display(Name="Item Code")]
        public string ItemCode { get; set; }
        [Display(Name="Category")]
        public string Category { get; set; }
        [Display(Name="Description")]
        public string Description { get; set; }
        [Display(Name = "Unit of Measure")]
        public string UOM { get; set; }
        [Display(Name = "In Store Quantity")]
        public int InStoreQty { get; set; }
        [Display(Name = "Low Stock Level")]
        public int ReorderLvl { get; set; }
        [Display(Name = "Reorder Quantity")]
        public int ReorderQty { get; set; }
        [Display(Name = "Tender Price")]
        public string TenderPrice { get; set; }
        public string Supplier1 { get; set; }
        public string Supplier2 { get; set; }
        public string Supplier3 { get; set; }
        public string LastOrder { get; set; }
        public string ItemNumber { get; set; }
        public string ImageURL { get; set; }

        public InventoryItemDetailsVM(InventoryItem item)
        {
            PurchaseService purchaseService = new PurchaseService();
            UnitOfWork unitOfWork = new UnitOfWork();

            ItemCode = item.ItemID;
            Category = item.Item.Category;
            Description = item.Item.Description;
            UOM = item.Item.UnitOfMeasure;
            InStoreQty = item.InStoreQty;
            ReorderLvl = item.ReorderLvl;
            ReorderQty = item.ReorderQty;
            ImageURL = item.Item.ImageURL;

            LastOrder = "No purchases in record";

            Tender[] top = purchaseService.topTender(item.Item);
            if (top != null)
            {
                Supplier1 = top[0].Supplier.ID ?? "";
                Supplier2 = top[1].Supplier.ID ?? "";
                Supplier3 = top[2].Supplier.ID ?? "";
            }
            PurchaseItem purchaseItem = purchaseService.recentPurchaseItem(item.Item) ?? null;
           
            if (purchaseItem != null)
            {
                PurchaseItem purchase = unitOfWork.PurchaseItemRepository.Get(filter: x => x.ID == purchaseItem.ID, includeProperties: "PurchaseOrder.CreatedByStaff, Tender.Supplier").First();
                string Supplier = purchase.Tender.Supplier.ID;
                PurchaseOrder PO = purchase.PurchaseOrder;
                if (PO != null)
                {
                    string POID = PO.ID.ToString();
                    string POStatus = PO.Status.ToString();
                    string OrderedBy = PO.CreatedByStaff.Name;
                    string OrderDate = PO.CreatedDate.ToString("dd/MM/yyyy");
                    LastOrder = "Last Purchase Order (" + POID + " / " + POStatus + ") to " + Supplier + " created by " + OrderedBy + " on " + OrderDate;
                }
                else
                {
                    if(purchase.Qty>0)
                        LastOrder = "Item currently in Cart for " + Supplier;
                }
                
            }
            Debug.WriteLine(LastOrder);

            ItemNumber = "1";
        }

    }
}