using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class RetrievalList : Document
    {
        public int DepartmentID { get; set; }

        public virtual ICollection<ItemTransaction> ItemTransactions { get; set; }

        public Department Department { get; set; }

        public RetrievalList(int creatorID, int responderID, DateTime createdDate, DateTime responseDate, Status status, int departmentID) : base(creatorID, responderID, createdDate, responseDate, status)
        {
            DepartmentID = departmentID;
        }
    }
}