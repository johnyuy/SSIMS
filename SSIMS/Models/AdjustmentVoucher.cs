using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class AdjustmentVoucher : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public AdjustmentVoucher(Staff creator) : base(creator)
        {
        }

        public AdjustmentVoucher() : base() { } 

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }

        public AdjustmentVoucher(AdjustmentVoucherVM AVM, UnitOfWork uow, Staff staff) : base()
        {
            if (AVM == null)
                return;
            CreatedByStaff = staff;
            List<DocumentItem> documentItems = new List<DocumentItem>();
            foreach(AdjustmentItemVM AItemVm in AVM.AdjustmentItems)
            {
                Item item = uow.ItemRepository.GetByID(AItemVm.ItemID);
                DocumentItem documentItem = new DocumentItem(item, AItemVm.QtyAdjusted);
                documentItem.Remarks = AItemVm.Remarks;
                documentItems.Add(documentItem);
            }
            DocumentItems = documentItems;
        }

    }
}