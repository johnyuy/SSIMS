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
            foreach(PurchaseItem p in purchaseitems)
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

            foreach( PurchaseItem p in purchaseItems)
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

    }
}