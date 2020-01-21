using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DeliveryOrder : Document
    {
        public virtual ICollection<DocumentItem> DocumentItems { get; set; }

        public DeliveryOrder(int creatorID, int responderID, DateTime createdDate, DateTime responseDate, Status status) : base(creatorID, responderID, createdDate, responseDate, status)
        {
        }

        public Boolean UpdateInventoryItem()
        {
            Console.WriteLine("UpdateInventoryItem()");
            return true;
        }
    }
}