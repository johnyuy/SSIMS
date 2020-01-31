using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class RequisitionSummaryViewModel
    {
        public RequisitionSummaryViewModel()
        {
        }

        public RequisitionSummaryViewModel(int qty, string category, DateTime createdDate, string department)
        {
            Qty = qty;
            Category = category;
            CreatedDate = createdDate;
            Department = department;
            Month = createdDate.Month;
        }

        [DisplayName("Quantity")]
        public int Qty { get; set; }
        [DisplayName("Category")]
        public string Category { get; set; }
        [DisplayName("Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Department")]
        public string Department { get; set; }
        [DisplayName("Month")]
        public int Month { get; set; }


    }
}