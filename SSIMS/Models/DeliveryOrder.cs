using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class DeliveryOrder : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public Supplier Supplier { get; set; }

        public DeliveryOrder() : base()
        {
        }

        public DeliveryOrder(Staff creator) : base(creator)
        {
        }

        public DeliveryOrder(int CreatedByStaffID, string supplierID, int purchaseOrderID, UnitOfWork uow) : base()
        {
            CreatedByStaff = uow.StaffRepository.GetByID(CreatedByStaffID);
            Supplier = uow.SupplierRepository.GetByID(supplierID);
            PurchaseOrder = uow.PurchaseOrderRepository.GetByID(purchaseOrderID);
            
        }

        public DeliveryOrder(Staff createdByStaff, Supplier supplier, PurchaseOrder purchaseOrder) : base()
        {
            CreatedByStaff = createdByStaff;
            Supplier = supplier;
            PurchaseOrder = purchaseOrder;

        }


        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}