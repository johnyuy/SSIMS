using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.ViewModels
{
    public class AdjustmentItemVM
    {
        [Display(Name = "Item Code")]
        public string ItemID { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Unit Of Measure")]
        public string UOM { get; set; }
        [Display(Name = "Qty Adjusted")]
        [DisplayFormat(DataFormatString = "{0:+#;-#;0}", ApplyFormatInEditMode = false)]
        public int QtyAdjusted { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public AdjustmentItemVM(DocumentItem documentItem)
        {
            ItemID = documentItem.Item.ID;
            Category = documentItem.Item.Category;
            Description = documentItem.Item.Description;
            UOM = documentItem.Item.UnitOfMeasure;
            QtyAdjusted = documentItem.Qty;
            Remarks = documentItem.Remarks;
        }

        public AdjustmentItemVM(string ItemId)
        {
            UnitOfWork uow = new UnitOfWork();
            Item item = uow.ItemRepository.GetByID(ItemId);
            if(item != null)
            {
                ItemID = item.ID;
                Category = item.Category;
                Description = item.Description;
                UOM = item.UnitOfMeasure;
                QtyAdjusted = 0;
                Remarks = "";
            }
        }

        public AdjustmentItemVM() { }
    }
}