using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;

namespace SSIMS.ViewModels
{
    public class ApiInventoryDetails
    {

        public string ItemID;
        public string ItemDesc;
        public string InStore;
        public string Disbursing;
        public string LowStock;
 
        public List<ApiStockCard> StockCards;

        public ApiInventoryDetails(string itemID, string itemDesc, string inStore, string disbursing, List<ApiStockCard> stockCards)
        {
            ItemID = itemID;
            ItemDesc = itemDesc;
            InStore = inStore;
            Disbursing = disbursing;
            this.StockCards = stockCards;
        }

        public ApiInventoryDetails(InventoryItem inventoryItem, UnitOfWork uow)
        {
            ItemID = inventoryItem.Item.ID;
            ItemDesc = inventoryItem.Item.Description;
            InStore = inventoryItem.InStoreQty.ToString();
            Disbursing = inventoryItem.InTransitQty.ToString();
            StockCards = new List<ApiStockCard>();

            if(inventoryItem.InStoreQty < inventoryItem.ReorderLvl)
            {
                LowStock = "Low Stock";
            }
            else
            {
                LowStock = "In Stock";
            }

            var sce = uow.StockCardEntryRepository.Get(filter: x => x.Item.ID == inventoryItem.Item.ID, includeProperties: "DeliveryOrder, RetrievalList, AdjustmentVoucher  ");     

            foreach(StockCardEntry se in sce)
            {
                StockCards.Add(new ApiStockCard(se));
            }
            

        }
    }
}