using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    public class RequisitionService: IRequisitionService
    {
        UnitOfWork uow = new UnitOfWork();
        public DocumentItem ConvertRequisitionItemVMToDocumentItem(RequisitionItemVM rivm)
        {
            if (rivm != null)
                return new DocumentItem(uow.ItemRepository.GetByID(rivm.SelectedDescription), rivm.Quantity);
            return null;
        }

        public void CreateNewRequistionOrder(List<RequisitionItemVM> requisitionItems, Staff creator)
        {
            if(requisitionItems != null)
            {
                if (requisitionItems.Count > 0)
                {
                    List<DocumentItem> documentItems = new List<DocumentItem>();
                    foreach(RequisitionItemVM rivm in requisitionItems)
                    {
                        documentItems.Add(ConvertRequisitionItemVMToDocumentItem(rivm));
                    }
                    RequisitionOrder requisitionOrder = new RequisitionOrder(creator);
                    requisitionOrder.DocumentItems = documentItems;
                    Debug.WriteLine("Adding RO for " + creator.Name);
                    uow.RequisitionOrderRepository.Insert(requisitionOrder);

                }
            }
                
        }
    }
}