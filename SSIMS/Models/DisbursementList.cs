using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DisbursementList : Document
    {
        public int DepartmentID { get; set; }
        public List<ItemTransaction> ItemTransactions { get; set; }

        public Department Department { get; set; }

        public DisbursementList(int creatorID, int responderID, DateTime createdDate, DateTime responseDate, Status status, int departmentID) : base(creatorID, responderID, createdDate, responseDate, status)
        {
            DepartmentID = departmentID;
        }
    }
}