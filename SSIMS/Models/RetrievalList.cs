using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class RetrievalList : Document
    {
        public virtual ICollection<TransactionItem> ItemTransactions { get; set; }

        public Department Department { get; set; }

        public RetrievalList(Staff creator, Department department) : base(creator)
        {
            Department = department;
        }

        public RetrievalList()
        {
        }
    }
}