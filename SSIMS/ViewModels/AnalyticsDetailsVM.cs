using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class AnalyticsDetailsVM
    {
        [DisplayName("Quantity")]
        public int Qty { get; set; }                                 //ADDABLE
        [DisplayName("Category")]
        public string Category { get; set; } //GROUP
        [DisplayName("Date")]
        public DateTime CreatedDate { get; set; }  
        [DisplayName("Department")]
        public string Department { get; set; } //GROUP
        [DisplayName("Month")]
        public int Month { get; set; } //GROUP
        [DisplayName("Year")]
        public int Year { get; set; }   //GROUP
        [DisplayName("Count")]
        public int Count { get; set; }
        [DisplayName("Item")]                                       //ADDABLE
        public string ItemCode { get; set; } //GROUP
        [DisplayName("Staff")]  
        public string OrderStaff { get; set; } //GROUP
        [DisplayName("Cost")]
        public double Cost { get; set; }                            //ADDABLE

        public AnalyticsDetailsVM()
        {
        }

        public AnalyticsDetailsVM(int qty, string category, DateTime createdDate, string department, string itemdesc, string staffname, double cost)
        {

            //Document Item Level (item + qty)
            Qty = qty;
            Category = category;
            CreatedDate = createdDate;
            Department = department;
            Month = createdDate.Month;
            Year = createdDate.Year;
            Count = 1;
            ItemCode = itemdesc;
            OrderStaff = staffname;
            Cost = cost;
        }

        public override string ToString()
        {
            return Qty + "/" + Category + "/" + Department + "/m" + Month + "/y" + Year + "/" + Count + "/" + ItemCode + "/" + OrderStaff + "/" + Cost.ToString($"${0:00}");
        }
    }
}