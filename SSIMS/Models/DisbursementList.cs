using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Service;

namespace SSIMS.Models
{
    public class DisbursementList : Document
    {
        public virtual ICollection<TransactionItem> ItemTransactions { get; set; }

        public Department Department { get; set; }
        public int OTP { get; set; }

        private DisbursementService ds = new DisbursementService();

        public DisbursementList(Staff creator, Department department) : base(creator)
        {
            Department = department;
            OTP = ds.GenerateOTP();
        }

        public DisbursementList() : base() { }

        public DisbursementList(ICollection<TransactionItem> itemTransactions, Department department)
        {
            ItemTransactions = itemTransactions;
            Department = department;

            OTP = ds.GenerateOTP();
        }

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}