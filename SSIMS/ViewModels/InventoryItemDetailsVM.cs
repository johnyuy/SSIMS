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
        public int Tender1 { get; set; }
        public int Tender2 { get; set; }
        public int Tender3 { get; set; }
        public string TenderPrice { get; set; }
        public string Supplier1 { get; set; }
        public string Supplier1Tender { get; set; }
        public string Supplier2 { get; set; }
        public string Supplier2Tender { get; set; }
        public string Supplier3 { get; set; }
        public string Supplier3Tender { get; set; }
        public string LastOrderLine1 { get; set; }
        public string LastOrderLine2 { get; set; }
        public string ImageURL { get; set; }
        public string UnitDisplay { get; set; }
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

            LastOrderLine1 = "No purchases in record";
            LastOrderLine2 = "";
            if (UOM == "Each")
                UnitDisplay = "Units";
            else
                UnitDisplay = UOM;

            Tender[] top = purchaseService.topTender(item.Item);
            if (top != null)
            {
                Tender1 = top[0].ID;
                Tender2 = top[1].ID;
                Tender3 = top[2].ID;
                Supplier1 = top[0].Supplier.ID ?? "";
                Supplier2 = top[1].Supplier.ID ?? "";
                Supplier3 = top[2].Supplier.ID ?? "";
                Supplier1Tender = top[0].Price.ToString($"${0:0.00}") ?? "na";
                Supplier2Tender = top[1].Price.ToString($"${0:0.00}") ?? "na";
                Supplier3Tender = top[2].Price.ToString($"${0:0.00}") ?? "na";

            }
            PurchaseItem purchaseItem = purchaseService.recentPurchaseItem(item.Item) ?? null;
           
            if (purchaseItem != null)
            {
                PurchaseItem purchase = unitOfWork.PurchaseItemRepository.Get(filter: x => x.ID == purchaseItem.ID, includeProperties: "PurchaseOrder.CreatedByStaff, Tender.Supplier").First();
                string Supplier = purchase.Tender.Supplier.ID;
                PurchaseOrder PO = purchase.PurchaseOrder;
                if (PO != null)
                {
                    string POID = PO.ID.ToString($"PO-{0:1000000}");
                    string POStatus = PO.Status.ToString();
                    string OrderedBy = PO.CreatedByStaff.Name;
                    string OrderDate = PO.CreatedDate.ToString("dd/MM/yyyy");
                    LastOrderLine1 = "Last Purchase " + POID + " (" + POStatus + ") to " + Supplier ;
                    LastOrderLine2 = "Created by " + OrderedBy + " on " + OrderDate;
                }
                else
                {
                    if (purchase.Qty > 0)
                    {
                        LastOrderLine1 = "Item currently in Purchase Cart for " + Supplier;
                        LastOrderLine2 = "Quantity Requested = " + purchase.Qty + " " + UnitDisplay;
                    }
                }
            }
            Debug.WriteLine(LastOrderLine1 + ", " + LastOrderLine2);
        }
    }
}