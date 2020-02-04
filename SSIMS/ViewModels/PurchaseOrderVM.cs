using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSIMS.ViewModels
{

    public class PurchaseOrderVM 
    {

        [Key]
        public int ID { get; set; }

        [ForeignKey("CreatedByStaff")]
        public int? CreatedByStaffID { get; set; }
        [ForeignKey("RepliedByStaff")]
        public int? RepliedByStaffID { get; set; }

        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public Models.Status Status { get; set; }

        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff RepliedByStaff { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }

        public double TotalCost { get; set; }       

        public PurchaseOrderVM(PurchaseOrder PO) 
        {
            this.ID = PO.ID;
            this.CreatedDate = PO.CreatedDate;
            this.TotalCost = GetTotalCost(PO);
            this.Status = PO.Status;
            this.PurchaseItems = PO.PurchaseItems;
            this.RepliedByStaff = PO.RepliedByStaff;
            this.ResponseDate = PO.ResponseDate;
            this.CreatedByStaff = PO.CreatedByStaff;
            this.Supplier = PO.Supplier;
            
        }

        public PurchaseOrderVM() : base()
        {
            CreatedDate = DateTime.Now;
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