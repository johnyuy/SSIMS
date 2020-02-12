using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiPurchaseOrderDetails
    {
        public string ID;

        public string CreatedByStaffID;
        public string CreatedByStaffName;
        public string RepliedByStaffID;
        public string RepliedByStaffName;

        public string Comments;
        public string CreatedDate;
        public string ResponseDate;
        public string Status;
        public string SupplierID;

        public string TotalCost;

        public List<ApiPurchaseItemView> PurchaseItems;

        public ApiPurchaseOrderDetails()
        {
        }

        public ApiPurchaseOrderDetails(string iD, string createdByStaffID, string createdByStaffName, string repliedByStaffID, string repliedByStaffName, string comments, string createdDate, string responseDate, string status, string totalCost, List<ApiPurchaseItemView> purchaseItems)
        {
            ID = iD;
            CreatedByStaffID = createdByStaffID;
            CreatedByStaffName = createdByStaffName;
            RepliedByStaffID = repliedByStaffID;
            RepliedByStaffName = repliedByStaffName;
            Comments = comments;
            CreatedDate = createdDate;
            ResponseDate = responseDate;
            Status = status;
            TotalCost = totalCost;
            PurchaseItems = purchaseItems;
        }

        public ApiPurchaseOrderDetails(PurchaseOrder PO)
        {
            ID = PO.ID.ToString();
            CreatedByStaffID = PO.CreatedByStaff.ID.ToString();
            CreatedByStaffName = PO.CreatedByStaff.Name.ToString();

            if (PO.RepliedByStaff == null)
            {
                RepliedByStaffID = "";
                RepliedByStaffName = "";
            }
            else
            {
                RepliedByStaffID = PO.RepliedByStaff.ID.ToString();
                RepliedByStaffName = PO.RepliedByStaff.Name.ToString();
            }
            Comments = PO.Comments;
            CreatedDate = PO.CreatedDate.ToString();
            ResponseDate = PO.ResponseDate.ToString();
            Status = PO.Status.ToString();
            TotalCost = "$"+ string.Format("{0:#.00}", Convert.ToDecimal(PO.TotalCost().ToString())); 
            SupplierID = PO.Supplier.ID;

            List<ApiPurchaseItemView> purchaseItemViews = new List<ApiPurchaseItemView>();

            foreach(PurchaseItem pi in PO.PurchaseItems)
            {
                purchaseItemViews.Add(new ApiPurchaseItemView(pi));
            }

            PurchaseItems = purchaseItemViews;

    }


    }
}