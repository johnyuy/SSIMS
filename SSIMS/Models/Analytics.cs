using SSIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Analytics
    {

       // public IEnumerable<RequisitionSummaryViewModel> categorylist { get;  set; }
        public IEnumerable<AnalyticsDetailsVM> categorylist { get; set; }
        public IEnumerable<AnalyticsDetailsVM> categorylist2 { get;  set; }
        public Analytics(List<AnalyticsDetailsVM> categorylist, List<AnalyticsDetailsVM> categorylist2)
        {
           
            this.categorylist = categorylist.ToList();
            this.categorylist2 = categorylist2.ToList();
        }

        
    }
}