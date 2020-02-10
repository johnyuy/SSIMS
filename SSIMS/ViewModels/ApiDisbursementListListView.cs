using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiDisbursementListListView
    {
        public string ID;
        public string DepartmentID;
        public string Status;
        public string Location;

        public ApiDisbursementListListView(DisbursementList disbursement, UnitOfWork uow)
        {
            ID = disbursement.ID.ToString();
            DepartmentID = disbursement.Department.ID;
            Status = disbursement.Status.ToString();
            Location = uow.DepartmentRepository.Get(filter: x=> x.ID == disbursement.Department.ID, includeProperties: "CollectionPoint").FirstOrDefault().CollectionPoint.Location;
        }
    }


}