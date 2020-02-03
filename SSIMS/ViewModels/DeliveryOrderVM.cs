using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SSIMS.Service;

namespace SSIMS.ViewModels
{
    public class DeliveryOrderVM
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

        //[NestedListValidation]
        public List<TransactionItem> PurchaseItems { get; set; }

        public double TotalCost { get; set; }

        public int PurchaseOrderID { get; set; }


        public DeliveryOrderVM(PurchaseOrder PO)
        {
            this.ID = PO.ID;
            this.CreatedDate = PO.CreatedDate;
            this.Status = PO.Status;
            this.PurchaseItems = PurchaseItemToTransactionItem(PO.PurchaseItems.ToList(), PO);
            this.RepliedByStaff = PO.RepliedByStaff;
            this.ResponseDate = PO.ResponseDate;
            this.CreatedByStaff = PO.CreatedByStaff;
            this.Supplier = PO.Supplier;
            this.PurchaseOrderID = PO.ID;
        }

        public DeliveryOrderVM() : base()
        {
            CreatedDate = DateTime.Now;
        }

        public TransactionItem PurchaseItemToTransactionItem(PurchaseItem pi, Document document)
        {
            TransactionItem ti = new TransactionItem(pi.Qty, pi.Qty, "", pi.Tender.Item, document);
            return ti;
        }

        public List<TransactionItem> PurchaseItemToTransactionItem(List<PurchaseItem> PIList, Document document)
        {
            List<TransactionItem> list = new List<TransactionItem>();
            foreach (PurchaseItem pi in PIList)
            {
                list.Add(PurchaseItemToTransactionItem(pi, document));

            }
            return list;
        }
    }
}