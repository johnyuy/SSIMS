﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DisbursementList : Document
    {
        public int? DepartmentID { get; set; }

        public virtual ICollection<TransactionItem> ItemTransactions { get; set; }

        public Department Department { get; set; }

        public DisbursementList(Staff creator, Department department) : base(creator)
        {
            Department = department;
        }

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}