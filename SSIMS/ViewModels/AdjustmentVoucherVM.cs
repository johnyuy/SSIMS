using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class AdjustmentVoucherVM
    {
        [DisplayFormat(DataFormatString ="AV{0:1000000}",ApplyFormatInEditMode =true)]
        [Display(Name="Adjustment ID")]
        public int AdjustmentID { get; set; }
        [Display(Name = "Reported By")]
        public string ReportedByStaffName { get; set; }
        public int ReportedByStaffID { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Date Created")]
        public string DateCreated { get; set; }
        public List<AdjustmentItemVM> AdjustmentItems { get; set; }

        public AdjustmentVoucherVM(AdjustmentVoucher AV)
        {
            AdjustmentID = AV.ID;
            ReportedByStaffID = AV.CreatedByStaff.ID;
            ReportedByStaffName = AV.CreatedByStaff.Name;
            Status = AV.Status.ToString();
            DateCreated = AV.CreatedDate.ToString("dd/MM/yyyy");
            List<AdjustmentItemVM> adjustmentItems = new List<AdjustmentItemVM>();
            foreach(DocumentItem documentItem in AV.DocumentItems)
            {
                adjustmentItems.Add(new AdjustmentItemVM(documentItem));
            }
            AdjustmentItems = adjustmentItems;
        }
        public AdjustmentVoucherVM()
        {
            AdjustmentItems = new List<AdjustmentItemVM>();
        }

    }
}