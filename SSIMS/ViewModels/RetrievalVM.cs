using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class RetrievalVM
    {
        public List<RetrievalItemViewModel> rivmlist { get; set; }
        public List<RequisitionOrder> ROList { get; set; }

        public RetrievalVM()
        {
            rivmlist = new List<RetrievalItemViewModel>();
        }

        public RetrievalVM(List<RetrievalItemViewModel> rivmlist, List<RequisitionOrder> rOList)
        {
            this.rivmlist = rivmlist;
            ROList = rOList;
        }
    }
}