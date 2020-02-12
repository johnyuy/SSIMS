using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiPurchaseItemView
    {
        public string ID;
        public string ItemID;
        public string ItemDesc;
        public string Qty;
        public string UnitPrice;
        public string Subtotal;


        public ApiPurchaseItemView()
        {
        }

        public ApiPurchaseItemView(string iD, string itemID, string itemDesc, string qty, string unitPrice, string subtotal)
        {
            ID = iD;
            ItemID = itemID;
            ItemDesc = itemDesc;
            Qty = qty;
            UnitPrice = unitPrice;
            Subtotal = subtotal;
        }

        public ApiPurchaseItemView(PurchaseItem pi)
        {
            ID = pi.ID.ToString();
            ItemID = pi.Tender.Item.ID;
            ItemDesc = pi.Tender.Item.Description;
            Qty = pi.Qty.ToString();
            UnitPrice = pi.Tender.Price.ToString();
            Subtotal = (pi.Tender.Price*pi.Qty).ToString();
        }


    }
}