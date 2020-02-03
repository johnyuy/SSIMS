using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class PurchaseOrderVM : PurchaseOrder
    {

        public double TotalCost { get; set; }       

        public PurchaseOrderVM(PurchaseOrder PO) :
            base(PO.CreatedByStaff, PO.Supplier)
        {
            this.ID = PO.ID;
            this.CreatedDate = PO.CreatedDate;
            this.TotalCost = GetTotalCost(PO);
            this.Status = PO.Status;
            this.PurchaseItems = PO.PurchaseItems;
            this.RepliedByStaff = PO.RepliedByStaff;
            this.ResponseDate = PO.ResponseDate;
            
        }

        public PurchaseOrderVM() : base()
        {
        }

        public double GetTotalCost(PurchaseOrder PO)
        {
            double total = 0;
            foreach (PurchaseItem p in PO.PurchaseItems)
            {
                total = total + (p.Qty * p.Tender.Price);
            }
            return total;
        }
    }



}