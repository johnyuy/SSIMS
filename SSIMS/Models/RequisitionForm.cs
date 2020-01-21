using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class RequisitionForm : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public RequisitionForm(int creatorID, int responderID, DateTime createdDate, DateTime responseDate, Status status) : base(creatorID, responderID, createdDate, responseDate, status)
        {
        }

        public Boolean UpdateRetrivalList()
        {
            Console.WriteLine("UpdateRetrivalList()");
            return true;
        }
    }
}