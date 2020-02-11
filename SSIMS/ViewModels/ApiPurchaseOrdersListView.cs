using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiPurchaseOrdersListView
    {
        public String ID;
        public String ClerkName;
        public String Status;
        public String ResponseDate;

        public ApiPurchaseOrdersListView()
        {
        }

        public ApiPurchaseOrdersListView(string iD, string clerkName, string status, string responseDate)
        {
            ID = iD;
            ClerkName = clerkName;
            Status = status;
            ResponseDate = responseDate;
        }

        public ApiPurchaseOrdersListView(PurchaseOrder PO)
        {
            ID = PO.ID.ToString();
            ClerkName = PO.CreatedByStaff.Name;
            Status = PO.Status.ToString();
            ResponseDate = PO.ResponseDate.ToString();

        }

    }
}