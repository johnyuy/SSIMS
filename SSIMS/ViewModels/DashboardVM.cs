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
        public List<DisbursementList> Disbursements { get; set; }

        public IEnumerable<InventoryItem> InventoryItems { get; set; }
        public DashboardVM()
        {

        }

        public DashboardVM(List<DisbursementList> dl, IEnumerable<InventoryItem> inventoryItems)
        {
            Disbursements = dl;
            InventoryItems = inventoryItems;
        }
    }
}