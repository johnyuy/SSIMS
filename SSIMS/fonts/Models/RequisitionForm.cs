using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class RequisitionForm : Document
    {
        public Dictionary<Item, int> RequestItemsMap { get; set; }
        public string Comments { get; set; }

        public RequisitionForm() 
        {


        }

        public RequisitionForm()
        {

        }

    }
}