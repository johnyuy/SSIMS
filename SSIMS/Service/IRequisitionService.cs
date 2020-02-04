using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    interface IRequisitionService
    {
        DocumentItem ConvertRequisitionItemVMToDocumentItem(RequisitionItemVM rivm);
        void CreateNewRequistionOrder(List<RequisitionItemVM> requisitionItems, Staff creator);

        List<RequisitionOrder> GetRequisitionOrdersbyStatus(Staff staff, Models.Status status);

        List<RequisitionOrder> GetRequisitionOrdersbyCreatedDate(Staff staff, DateTime dateTime);




    }
}