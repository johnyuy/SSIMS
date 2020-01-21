using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class RequisitionForm : Document
    {
        public Dictionary<ItemID, int> RequisitionItemMap {get;set;}
        public string Comment { get; set; }

    }
}