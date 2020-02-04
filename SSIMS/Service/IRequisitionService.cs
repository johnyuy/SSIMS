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
    }
}