using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class PurchaseOrder : Document
    {
        

        public DateTime? ExpectedDeliveryDate { get; set; }

        public Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }

        public PurchaseOrder() : base()
        {
        }

        public PurchaseOrder(Staff creator) : base(creator)
        {
            PurchaseItems = new List<PurchaseItem>();
        }

        public PurchaseOrder(Staff creator, Supplier supplier) : base(creator)
        {
            Supplier = supplier;
            ExpectedDeliveryDate = null;
        }

        public PurchaseOrder(Staff creator, Supplier supplier, DateTime expectedDeliveryDate) : base(creator)
        {
            Supplier = supplier;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }

        public PurchaseOrder(int CreatedByStaffID, string supplierID, UnitOfWork uow) : base()
        {
            CreatedByStaff = uow.StaffRepository.GetByID(CreatedByStaffID);
            Supplier = uow.SupplierRepository.GetByID(supplierID);
            ExpectedDeliveryDate = null;
        }

        public PurchaseOrder(int CreatedByStaffID, string supplierID, DateTime expectedDeliveryDate, UnitOfWork uow) : base()
        {
            CreatedByStaff = uow.StaffRepository.GetByID(CreatedByStaffID);
            Supplier = uow.SupplierRepository.GetByID(supplierID);
            ExpectedDeliveryDate = expectedDeliveryDate;
        }

        public double TotalCost()
        {
            double total = 0;
            foreach (PurchaseItem p in PurchaseItems)
            {
                total = total + (p.Qty * p.Tender.Price);
            }
            return total;
        }

    }
}