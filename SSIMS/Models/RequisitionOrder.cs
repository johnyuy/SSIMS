using System;
using System.Collections.Generic;

namespace SSIMS.Models
{
    public class RequisitionOrder : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public RequisitionOrder(Staff creator) : base(creator)
        {

        }

        public Boolean UpdateRetrivalList()
        {
            Console.WriteLine("UpdateRetrivalList()");
            return true;
        }
    }
}