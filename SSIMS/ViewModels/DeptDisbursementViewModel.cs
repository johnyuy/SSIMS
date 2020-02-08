using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.ViewModels
{
    public class DeptDisbursementViewModel
    {
        [DisplayName("Dept ID")]
        public string deptID { get; set; }
        public List<TransactionItem> transItemLists { get; set; }
        [DisplayName("Status")]
        public Status status { get; set; }

        public DeptDisbursementViewModel()
        {
        }

        public DeptDisbursementViewModel(string deptID, List<TransactionItem> transItemLists)
        {
            this.deptID = deptID;
            this.transItemLists = transItemLists;
            status = Status.Pending;
        }
    }
}