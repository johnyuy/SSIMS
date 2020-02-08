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
            //show only the latest 100 results
            List<AdjustmentVoucherVM> last100 = Enumerable.Reverse(VMList).Take(100).Reverse().ToList();
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
    }
}