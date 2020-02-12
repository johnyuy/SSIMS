using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SSIMS.DAL;

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
        public string Item { get; set; } //GROUP
        [DisplayName("Staff")]  
        public string Staff { get; set; } //GROUP
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
            Item = itemdesc;
            Staff = staffname;
            Cost = cost;
        }

        public override string ToString()
        {
            return Qty + "/" + Category + "/" + Department + "/m" + Month + "/y" + Year + "/" + Count + "/" + Item + "/" + Staff + "/" + Cost.ToString($"${0:00}");
        }


        public AnalyticsDetailsVM(int qty, string category, string createdDate, string department, string itemdesc, string staffname, UnitOfWork uow)
        {

            //Document Item Level (item + qty)
            Qty = qty;
            Category = category;
            CreatedDate = DateTime.Parse(createdDate);
            Department = department;
            Month = CreatedDate.Month;
            Year = CreatedDate.Year;
            Count = 1;
            Item = itemdesc;
            Staff = staffname;
            Cost = uow.TenderRepository.GetSampleTenderPrice(itemdesc);
        }


        
    }
}