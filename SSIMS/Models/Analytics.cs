using SSIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Analytics
    {

        public IEnumerable<RequisitionSummaryViewModel> categorylist { get;  set; }
        public IEnumerable<RequisitionSummaryViewModel> categorylist2 { get;  set; }
        public Analytics(List<RequisitionSummaryViewModel> categorylist, List<RequisitionSummaryViewModel> categorylist2)
        {
           
            this.categorylist = categorylist;
            this.categorylist2 = categorylist2;
        }

        
    }
}