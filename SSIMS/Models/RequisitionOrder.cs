using System;
using System.Collections.Generic;

namespace SSIMS.Models
{
    public class RequisitionOrder : Document
    {
        public ICollection<DocumentItem> DocumentItems { get; set; }

        public RequisitionOrder(Staff creator) : base(creator)
        {
            this.DocumentItems = new List<DocumentItem>();
        }
        public RequisitionOrder() : base()
        {
            this.DocumentItems = new List<DocumentItem>();
        }
        public Boolean UpdateRetrivalList()
        {
            Console.WriteLine("UpdateRetrivalList()");
            return true;
        }
    }
}