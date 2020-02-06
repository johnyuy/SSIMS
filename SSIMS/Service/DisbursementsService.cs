using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;
using System.Collections;
using SSIMS.Database;
using SSIMS.ViewModels;


namespace SSIMS.Service
{
    public class DisbursementsService
    {
        UnitOfWork uow = new UnitOfWork();

        public List<RetrievalListItemVM> GenerateDeptRequisitionList(string deptID, bool toDisburse = false)
        {
            //filter by dept & status
            List<RequisitionOrder> ROList = (List<RequisitionOrder>)uow.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.DepartmentID == deptID && x.Status == Models.Status.Approved, includeProperties: "CreatedByStaff,DocumentItems.Item");
            //update RO status if its a confirmed retrieval for disbursement
            if(toDisburse)
                CompleteRequisitions(ROList);

            if (ROList == null || ROList.Count==0)
                return null;
            
            List<DocumentItem> DIList = new List<DocumentItem>();
            foreach (RequisitionOrder RO in ROList)
            {
                foreach (DocumentItem DI in RO.DocumentItems)
                {
                    DIList.Add(DI);
                }
            }

            List<RetrievalListItemVM> EvaluatedList = new List<RetrievalListItemVM>();

            foreach(string itemid in DIList.Select(o => o.Item.ID).Distinct())
            {
                EvaluatedList.Add(new RetrievalListItemVM(itemid, 0));
            }

            foreach(RetrievalListItemVM itemVM in EvaluatedList)
            {
                foreach(DocumentItem di in DIList)
                {
                    if(di.Item.ID.Equals(itemVM.ItemID))
                    {
                        itemVM.HandoverQty += di.Qty;
                    }
                }
            }

            return EvaluatedList;
        }

        public List<RetrievalListVM> GenerateFullRetrievalList(bool toDisburse=false)
        {
            List<Department> departments = (List<Department>) uow.DepartmentRepository.Get();
            if (departments == null || departments.Count==0)
                return null;

            List<RetrievalListVM> FullList = new List<RetrievalListVM>();
            foreach(Department department in departments)
            {
                
                List<RetrievalListItemVM> departmentRL = GenerateDeptRequisitionList(department.ID,toDisburse);
                if (departmentRL == null)
                    continue;
                FullList.Add(new RetrievalListVM(department.DeptName, departmentRL));
            }

            if (toDisburse)
            {
                Staff creator = new LoginService().StaffFromSession;
                CreateRetrieval(FullList, creator);
            }
                
            return FullList;
        }

        private void CreateRetrieval(List<RetrievalListVM> FullList, Staff creator)
        {
            if (FullList == null || creator == null || FullList.Count==0)
                return;
            foreach (RetrievalListVM retrievalListVM in FullList) {
                List<RetrievalListItemVM> DepartmentRL = retrievalListVM.DepartmentRetrievalList;
                if (DepartmentRL == null || DepartmentRL.Count == 0)
                    continue;
                Department department = uow.DepartmentRepository.Get(x => x.DeptName == retrievalListVM.Department).First();
                RetrievalList retrievalList = new RetrievalList(creator, department);
                List<TransactionItem> transactionItems = new List<TransactionItem>();
                foreach(RetrievalListItemVM item in DepartmentRL)
                {
                    transactionItems.Add(new TransactionItem(item.HandoverQty, item.TakeoverQty, item.Reason, uow.ItemRepository.GetByID(item.ItemID)));
                }
                retrievalList.ItemTransactions = transactionItems;
                retrievalList.Status = Models.Status.InProgress;
                uow.RetrievalListRepository.Insert(retrievalList);
                uow.Save();
            }
        }

        private void CompleteRequisitions(List<RequisitionOrder> ROList)
        {
            foreach(RequisitionOrder RO in ROList)
            {
                RO.Status = Models.Status.Completed;
                uow.RequisitionOrderRepository.Update(RO);
                uow.Save();
            }
        }
    }
}