using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DeliveryOrder : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public DeliveryOrder(Staff creator) : base(creator)
        {
        }

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}