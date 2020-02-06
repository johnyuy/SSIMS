using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class RetrievalItemViewModel
    {
        public RetrievalItemViewModel(Item item, TransactionItem transactionItem, List<DeptRetrievalItemViewModel> deptRetrievalItems)
        {
            this.item = item;
            this.transactionItem = transactionItem;
            this.deptRetrievalItems = deptRetrievalItems;
        }
        public RetrievalItemViewModel(Item item, TransactionItem transactionItem)
        {
            this.item = item;
            this.transactionItem = transactionItem;
            this.deptRetrievalItems = new List<DeptRetrievalItemViewModel>();
        }

        public RetrievalItemViewModel()
        {
        }

        public Item item { get; set; }
        public TransactionItem transactionItem { get; set; }
        public List<DeptRetrievalItemViewModel> deptRetrievalItems { get; set; }

        
    }
}