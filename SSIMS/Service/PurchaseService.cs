using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;
using System.Collections;
using SSIMS.Database;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    public class PurchaseService
    {
        static UnitOfWork unitOfWork = new UnitOfWork();

        public List<PurchaseItem> GetPurchaseItemsForPO()
        {
            Console.WriteLine("GetPurchaseItemsForPO");


            var purchaseitems = unitOfWork.PurchaseItemRepository.GetWithRawSql("SELECT * FROM PurchaseItems WHERE PurchaseOrder_ID IS NULL");
            foreach (PurchaseItem p in purchaseitems)
            {
                Debug.WriteLine(p.Tender.ID.ToString());
            }
            return purchaseitems.ToList();

        }

        public Tender GetTender(int tenderID)
        {
            Console.WriteLine("GetPurchaseItemsForPO");


            Tender Tender = unitOfWork.TenderRepository.Get(filter: x => x.ID == tenderID, includeProperties: "Item").FirstOrDefault();

            return Tender;

        }

        public List<PurchaseItemVM> InitializePurchaseItemMV(List<PurchaseItem> purchaseItems)
        {
            List<PurchaseItemVM> mvs = new List<PurchaseItemVM>();

            foreach (PurchaseItem p in purchaseItems)
            {
                PurchaseItemVM mv = new PurchaseItemVM(p, unitOfWork);
                mvs.Add(mv);
            }
            return mvs;

        }

        public List<PurchaseItemVM> FetchCurrentPuchaseItems()
        {
            List<PurchaseItemVM> mvs = InitializePurchaseItemMV(GetPurchaseItemsForPO());

            return mvs;
        }

        public Tender[] topTender(Item item)
        {
            Tender[] top = new Tender[3];
            List<Tender> tenders = unitOfWork.TenderRepository.GetWithRawSql("SELECT TOP (3) ID, Price, Item_ID, Supplier_ID  FROM Tenders WHERE Item_ID = 'C001' Order By Price").ToList();
            if (tenders != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    top[i] = tenders[i];
                }
            }
            return top;

        }

        public PurchaseItem recentPurchaseItem(Item item)
        {
            PurchaseItem purchaseItem = new PurchaseItem();

            purchaseItem = unitOfWork.PurchaseItemRepository.GetWithRawSql("SELECT PurchaseItems.ID, Qty, PurchaseOrder_ID, Tender_ID FROM PurchaseItems, PurchaseOrders, Tenders, Items  WHERE PurchaseOrder_ID = PurchaseOrders.ID AND PurchaseItems.Tender_ID = Tenders.ID AND Tenders.Item_ID = Items.ID AND Item_ID = @p0 ORDER BY PurchaseOrders.CreatedDate DESC", item.ID).FirstOrDefault();

            if (purchaseItem == null)
            {
                purchaseItem = unitOfWork.PurchaseItemRepository.Get(x => x.Tender.Item.ID == item.ID && x.PurchaseOrder == null).FirstOrDefault();
            }
            return purchaseItem;
        }
    }
}