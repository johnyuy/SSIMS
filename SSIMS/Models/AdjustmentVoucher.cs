using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class AdjustmentVoucher : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public AdjustmentVoucher(Staff creator) : base(creator)
        {
        }

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}