using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Database;

namespace SSIMS.ViewModels
{
    public class PurchaseItemVM 
    {
        public int ID { get; set; }
        public int StoreQty { get; set; }
        public int Qty { get; set; }
        public Tender Tender { get; set; }

        public PurchaseItemVM(PurchaseItem PI, UnitOfWork uow) : base()
        {
            ID = PI.ID;
            Qty = PI.Qty;
            Tender = PI.Tender;
            StoreQty = uow.InventoryItemRepository.Get(filter: x => x.ItemID == Tender.Item.ID).FirstOrDefault().InStoreQty;
        }

    }
}