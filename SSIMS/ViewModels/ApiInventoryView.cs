using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiInventoryView
    {
        public string ItemID;
        public string ItemDesc;
        public string InStore;
        public string Disbursing;

        public ApiInventoryView()
        {
        }

        public ApiInventoryView(string itemID, string itemDesc, string inStore, string disbursing)
        {
            ItemID = itemID;
            ItemDesc = itemDesc;
            InStore = inStore;
            Disbursing = disbursing;
        }

        public ApiInventoryView(InventoryItem II)
        {
            ItemID = II.Item.ID;
            ItemDesc = II.Item.Description;
            InStore = II.InStoreQty.ToString();
            Disbursing = II.InTransitQty.ToString();

        }
    }
}