using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiDisbursementListLogView
    {
        public string ID;
        public string DepartmentID;
        public string Status;
        public string Date;

        public ApiDisbursementListLogView(DisbursementList disbursement)
        {
            ID = disbursement.ID.ToString();
            DepartmentID = disbursement.Department.ID;
            Status = disbursement.Status.ToString();
            Date = disbursement.ResponseDate.ToString();
        }
    }
}