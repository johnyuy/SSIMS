using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class AnalyticsDetailsVM
    {
        public AnalyticsDetailsVM()
        {
        }

        public AnalyticsDetailsVM(int qty, string category, DateTime createdDate, string department, string itemdesc, string staffname)
        {
            Qty = qty;
            Category = category;
            CreatedDate = createdDate;
            Department = department;
            Month = createdDate.Month;
            Count = 1;
            ItemName = itemdesc;
            OrderStaff = staffname;
            
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
        [DisplayName("Order Count")]
        public int Count { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Staff")]
        public string OrderStaff { get; set; }
    }
}