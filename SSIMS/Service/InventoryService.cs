using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;

namespace SSIMS.Service
{
    public class InventoryService
    {
        UnitOfWork uow = new UnitOfWork();
        public static int GetItemIndexFromSearchList(int inventoryItemID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 1;

            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 1;

            for (int i = 0; i < list.Count; i++)
            {
                if (inventoryItemID == list[i].ID)
                    return i + 1;
            }
            return 1;
        }
        public static int GetNextIndexFromSearchList(int currentID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 0;
            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (currentID == list[i].ID && i < list.Count - 1)
                    return list[i + 1].ID;
            }
            return 0;
        }
        public static int GetPrevIndexFromSearchList(int currentID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 0;
            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (currentID == list[i].ID && i > 0)
                    return list[i - 1].ID;
            }
            return 0;
        }

        public static void addItemToAdjustmentCart(AdjustmentItemVM ItemToAdd)
        {
            if (HttpContext.Current.Session["AdjustmentCart"] == null)
                HttpContext.Current.Session["AdjustmentCart"] = new AdjustmentVoucherVM();
            AdjustmentVoucherVM Voucher = (AdjustmentVoucherVM)HttpContext.Current.Session["AdjustmentCart"];
            Voucher.AdjustmentItems.Add(ItemToAdd);
        }

        public static bool VerifyAdjustmentCart()
        {
            if (HttpContext.Current.Session["AdjustmentCart"] == null)
                return false;
            AdjustmentVoucherVM Voucher = (AdjustmentVoucherVM)HttpContext.Current.Session["AdjustmentCart"];
            foreach(AdjustmentItemVM item in Voucher.AdjustmentItems)
            {
                if (item.QtyAdjusted.Equals(0))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ProcessStockAdjustmentEntry(Staff staff)
        {
            UnitOfWork uow = new UnitOfWork();
            if (HttpContext.Current.Session["AdjustmentCart"] == null)
                return false;
            AdjustmentVoucherVM Voucher = (AdjustmentVoucherVM)HttpContext.Current.Session["AdjustmentCart"];
            AdjustmentVoucher adjustmentVoucher = new AdjustmentVoucher(Voucher, uow, staff);
            if(uow.StockCardEntryRepository.ProcessAdjustmentVoucher(adjustmentVoucher))
            {
                uow.AdjustmentVoucherRepository.Insert(adjustmentVoucher);
                uow.Save();
                Debug.WriteLine("Adjustment Voucher inserted successfully into DB");
                //Update the respective inventory item
                foreach(AdjustmentItemVM adjustment in Voucher.AdjustmentItems)
                {
                    if (!UpdateInStoreQty(adjustment.ItemID))
                        Debug.WriteLine("Update In Store Qty failed for" + adjustment.ItemID);
                }

                return true;
            }

            return false;
        }


        public bool ProcessRejectedDisbursement(Staff staff, int DisbursementID)
        {
            UnitOfWork uow = new UnitOfWork();

            DisbursementList dl = uow.DisbursementListRepository.Get(filter: x => x.ID == DisbursementID, includeProperties: "ItemTransactions.Item").FirstOrDefault();

            if (dl == null) return false;
            
            AdjustmentVoucher adjustmentVoucher = new AdjustmentVoucher(dl, uow, staff);
            if (adjustmentVoucher == null) return false;

            if (uow.StockCardEntryRepository.ProcessAdjustmentVoucher(adjustmentVoucher))
            {
                uow.AdjustmentVoucherRepository.Insert(adjustmentVoucher);
                uow.Save();
                Debug.WriteLine("Adjustment Voucher inserted successfully into DB for rejected DL");
                //Update the respective inventory item
                foreach (DocumentItem di in adjustmentVoucher.DocumentItems)
                {
                    if (!UpdateInStoreQty(di.Item.ID))
                        Debug.WriteLine("Update In Store Qty failed for" + di.Item.ID);
                }
                return true;
            }
            return false;
        }

        public List<AdjustmentVoucherVM> GetAdjustmentVoucherVMList()
        {
            List<AdjustmentVoucher> adjustmentVouchers = uow.AdjustmentVoucherRepository.Get(includeProperties: "CreatedByStaff,DocumentItems.Item").ToList();
            if (adjustmentVouchers == null)
                return null;
            List<AdjustmentVoucherVM> VMList = new List<AdjustmentVoucherVM>();
            foreach (AdjustmentVoucher adjustmentVoucher in adjustmentVouchers)
            {
                VMList.Add(new AdjustmentVoucherVM(adjustmentVoucher));
            }
            VMList.Sort((x, y) => x.Status.CompareTo(y.Status));
            //show only the latest 100 results
            List<AdjustmentVoucherVM> last100 = Enumerable.Reverse(VMList).Take(100).ToList();

            return last100;
        }

        public AdjustmentVoucherVM GetAdjustmentVoucherVMSingle(int id)
        {
            AdjustmentVoucher voucher = uow.AdjustmentVoucherRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff,DocumentItems.Item").FirstOrDefault();
            if (voucher == null)
                return null;
            AdjustmentVoucherVM vM = new AdjustmentVoucherVM(voucher);
            return vM;
        }

        public void UpdateAdjustmentVoucherStatus(int id, bool isApproved, Staff responseStaff)
        {
            UnitOfWork uow = new UnitOfWork();

            //For adjustment voucher, can only be approved
            AdjustmentVoucher voucher = uow.AdjustmentVoucherRepository.GetByID(id);
            if (voucher == null)
                return;
            if(isApproved){
                voucher.Approve(responseStaff);
            }
            uow.AdjustmentVoucherRepository.Update(voucher);
            uow.Save();
            Debug.WriteLine("Adjustment Voucher ID " + voucher.ID + " has been approved by " + responseStaff.Name);
        }

        public List<InventoryCheckVM> GenerateInventoryCheckList()
        {
            UnitOfWork uow = new UnitOfWork();
            List<InventoryItem> inventoryItems = uow.InventoryItemRepository.Get(includeProperties: "Item").ToList();
            if(inventoryItems == null || inventoryItems.Count == 0)
                return null;

            List<InventoryCheckVM> VMList = new List<InventoryCheckVM>();
            foreach(InventoryItem item in inventoryItems)
            {
                VMList.Add(new InventoryCheckVM(item));
            }
            return VMList;
        }

        public bool UpdateInStoreQty(string ItemID)
        {
            UnitOfWork uow = new UnitOfWork();
            IEnumerable<StockCardEntry> entries= uow.StockCardEntryRepository.Get(filter: x => x.Item.ID == ItemID, includeProperties: "Item");

            if (entries == null || entries.Count() == 0)
                return false;

            int balance = entries.Sum(x => x.QtyChanged);

            InventoryItem item = uow.InventoryItemRepository.Get(x => x.ItemID == ItemID).FirstOrDefault();
            if (item == null)
                return false;
            item.InStoreQty = balance;

            uow.InventoryItemRepository.Update(item);
            uow.Save();
            Debug.WriteLine("InStore Qty for " + ItemID + "updated to" + balance);
            return true;
        }

        public bool UpdateReorderLvl(string ItemID, int newlvl)
        {
            if (newlvl < 0) return false;

            UnitOfWork uow = new UnitOfWork();
            InventoryItem inventoryItem = uow.InventoryItemRepository.Get(filter: x => x.ItemID == ItemID).FirstOrDefault();
            if (inventoryItem == null) return false;

            inventoryItem.ReorderLvl = newlvl;
            uow.InventoryItemRepository.Update(inventoryItem);
            uow.Save();

            return true;
        }

        public bool UpdateReorderQty(string ItemID, int newqty)
        {
            if (newqty < 1) return false;

            UnitOfWork uow = new UnitOfWork();
            InventoryItem inventoryItem = uow.InventoryItemRepository.Get(filter: x => x.ItemID == ItemID).FirstOrDefault();
            if (inventoryItem == null) return false;

            inventoryItem.ReorderQty = newqty;
            uow.InventoryItemRepository.Update(inventoryItem);
            uow.Save();

            return true;
        }
    }
}