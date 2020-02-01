using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class DeptRetrievalItemViewModel
    {
        public string deptID { get; set; }
        public TransactionItem transactionItem { get; set; }


        public DeptRetrievalItemViewModel(string deptID, TransactionItem transactionItem)
        {
            this.deptID = deptID;
            this.transactionItem = transactionItem;
        }
    }
}