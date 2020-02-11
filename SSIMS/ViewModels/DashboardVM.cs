using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.ViewModels
{
    public class DashboardVM
    {
        public List<DisbursementList> disbursements { get; set; }

        public IEnumerable<InventoryItem> inventoryItems { get; set; }
        public DashboardVM()
        {

        }
    }
}