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

        public List<RequisitionItemVM> ConvertDocumentItemsToRequisitionItems(List<DocumentItem> documentItems)
        {
            List<RequisitionItemVM> ritems = new List<RequisitionItemVM>();
           
            if (documentItems != null)
            {
                foreach(DocumentItem ditem in documentItems)
                {
                    RequisitionItemVM rtem = new RequisitionItemVM();
                    rtem.DoitemID = ditem.ID;
                    rtem.Quantity = ditem.Qty;
                    rtem.SelectedCategory = ditem.Item.Category;
                    rtem.SelectedDescription = ditem.Item.ID;
                    rtem.displayDescription = ditem.Item.Description;
                    rtem.UnitOfMeasure = ditem.Item.UnitOfMeasure;
                    
                    ritems.Add(rtem);
                    
                }

                return ritems;
            }

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

        public List<RequisitionOrder> GetRequisitionOrdersbyStatus(Staff staff, Models.Status status)
        {
            List<RequisitionOrder> ros = new List<RequisitionOrder>();
            List<RequisitionOrder> allros = uow.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff == staff).ToList();
            if (status != null)
            {
                foreach (RequisitionOrder ro in allros)
                {
                    if (ro.Status == status)
                    {
                        ros.Add(ro);
                    }
                }
                return ros;
            }

            return allros;
        }

        public List<RequisitionOrder> GetRequisitionOrdersbyCreatedDate(Staff staff,DateTime dateTime)
        {
            List<RequisitionOrder> ros = new List<RequisitionOrder>();
            List<RequisitionOrder> allros = uow.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff == staff).ToList();
            foreach(RequisitionOrder ro in allros)
            {
                if (ro.CreatedDate.Day == dateTime.Day) {
                    ros.Add(ro);
                }
            }
            return ros;
        }

    }
}