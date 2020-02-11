using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiRequisitionListView
    {
        public string ID;
        public string StaffName;
        public string Status;
        public string CreatedDate;

        public ApiRequisitionListView()
        {
        }

        public ApiRequisitionListView(string iD, string clerkName, string status, string responseDate)
        {
            ID = iD;
            StaffName = clerkName;
            Status = status;
            CreatedDate = responseDate;
        }

        public ApiRequisitionListView(RequisitionOrder RO)
        {
            ID = RO.ID.ToString();
            StaffName = RO.CreatedByStaff.Name;
            Status = RO.Status.ToString();
            CreatedDate = RO.CreatedDate.ToString();

        }

    }
}