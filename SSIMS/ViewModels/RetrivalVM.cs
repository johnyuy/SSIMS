using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class RetrivalVM
    {
        public List<RetrievalItemViewModel> rivmlist { get; set; }
        public List<RequisitionOrder> ROList { get; set; }

        public RetrivalVM()
        {
            rivmlist = new List<RetrievalItemViewModel>();
        }

        public RetrivalVM(List<RetrievalItemViewModel> rivmlist, List<RequisitionOrder> rOList)
        {
            this.rivmlist = rivmlist;
            ROList = rOList;
        }
    }
}