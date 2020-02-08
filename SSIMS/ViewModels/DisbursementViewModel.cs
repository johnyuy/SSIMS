using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class DisbursementViewModel
    {
        public List<DeptDisbursementViewModel> deptDVM { get; set; }
        public DisbursementViewModel()
        {
        }

        public DisbursementViewModel(List<DeptDisbursementViewModel> deptDVM)
        {
            this.deptDVM = deptDVM;
        }
    }
}