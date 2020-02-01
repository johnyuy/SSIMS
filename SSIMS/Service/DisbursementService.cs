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
    public class DisbursementService
    {
        static UnitOfWork unitOfWork = new UnitOfWork();

        //returns a list of TransactionIems from approved Request Orders of a given dept
        public List<TransactionItem> GenerateDeptRetrievalList(string deptID)
        {
            //requisitionOrders are approved
            
           
            List<DocumentItem> docItemsArray = new List<DocumentItem>();
            ICollection<TransactionItem> retrievalList;

            //retrieve all RO with status approved
            List<RequisitionOrder> ROList = new List<RequisitionOrder>();
            var approvedRO = unitOfWork.RequisitionOrderRepository.Get(includeProperties:"DocumentItems.Item,CreatedByStaff.Department" ,filter: x => x.Status.ToString() == "Approved");
            
            //filter by department
            foreach (RequisitionOrder RO in approvedRO)
            {
                Debug.WriteLine("RO ID: " + RO.ID);
                string deptRO = RO.CreatedByStaff.Department.ID;
                Debug.WriteLine("RO is from dept: " + deptRO);
                if (deptRO.Equals(deptID))
                {
                    ROList.Add(RO);
                }

                Debug.WriteLine("Number of elements in ROList: " + ROList.Count);
            }

            

            //get all ROs DI into a combined list
            List<DocumentItem> combinedDocList = new List<DocumentItem>();
            for (int i=0; i<ROList.Count; i++)
            {
                var docList = ROList[i].DocumentItems;
                foreach (DocumentItem doc in docList)
                {
                   combinedDocList.Add(doc);
                }
            }
            Debug.WriteLine("Combined Doc List size: " + combinedDocList.Count);
            List<TransactionItem> tempTransItems = new List<TransactionItem>();
            List<TransactionItem> transItemList = new List<TransactionItem>();

            if (transItemList.Count == 0 && combinedDocList.Count>0)
            {
                TransactionItem item = new TransactionItem(0,0, null, combinedDocList[0].Item);
                tempTransItems.Add(item);
                Debug.WriteLine("first temp trans item");
            }
            //group all DI by item.ID, construct TI with sum of qty in DIs
            //add TI into a list
            foreach (DocumentItem doc in combinedDocList)
            {
                bool isExist = false;
                TransactionItem temp;
                int index = 0;
                    for (int i = 0; i < transItemList.Count; i++)
                    {
                        temp = transItemList[i];
                        if (temp.Item.ID.Equals(doc.Item.ID))
                        {
                            isExist = true;
                            index = i;
                            break;
                        }
                      
                    }
                if (isExist)
                {
                    transItemList[index].HandOverQty += doc.Qty;
                    transItemList[index].TakeOverQty += doc.Qty;
                }
                else
                {
                    TransactionItem item = new TransactionItem(doc.Qty, doc.Qty, "Retrieval", doc.Item);
                    transItemList.Add(item);
                }
            }

            foreach (TransactionItem item in transItemList)
            {
                Debug.WriteLine("TransItem List: " + item.Item.Description + " " + item.HandOverQty);
            }
            return transItemList;
        }

        
        //same as GenerateDeptRetrievalList but with insert into Database
        public void InsertDeptRetrievalList(string deptID)
        {
            Staff clerk = unitOfWork.StaffRepository.GetByID(10003);
            Department dept = unitOfWork.DepartmentRepository.GetByID(deptID);
            List<TransactionItem> deptRetrievalList = GenerateDeptRetrievalList(deptID);
            RetrievalList retrievalList = new RetrievalList(clerk, dept);
            retrievalList.ItemTransactions = deptRetrievalList;
            retrievalList.Status = (Models.Status)5;
            Debug.WriteLine("Saving to RLrepo...");
            unitOfWork.RetrievalListRepository.Insert(retrievalList);
            unitOfWork.Save();
        }

        //generate the retrieval list item for the model view
        public DeptRetrievalItemViewModel GenerateDeptRetrievalItem(Department d, Item item)
        {

            // find transactional item using dept and item and in progress
            List<TransactionItem> deptRetrievalList = GenerateDeptRetrievalList(d.ID);
            TransactionItem tItem = (TransactionItem)deptRetrievalList.Where(x => x.Item == item);

            // using found item to construct DeptRetrievalItem
            DeptRetrievalItemViewModel deptRetrievalItem = new DeptRetrievalItemViewModel(d.ID, tItem);

            return deptRetrievalItem;
        }

        //generate the retrieval list item for the model view using Retrieval List as input
        public List<DeptRetrievalItemViewModel> GenerateDeptRetrievalItemListByRetrievalList(RetrievalList retrievalList)
        {
            List<TransactionItem> itemList = (List<TransactionItem>)retrievalList.ItemTransactions;
            List<DeptRetrievalItemViewModel> drivmList = new List<DeptRetrievalItemViewModel>();
            foreach (TransactionItem item in itemList)
            {
                DeptRetrievalItemViewModel drvm = new DeptRetrievalItemViewModel(retrievalList.Department.ID, item);
                drivmList.Add(drvm);
            }
            return drivmList;
        }

        //generate RetrievalItemViewwModel i.e. the item for the combined retrieval list 
        public List<RetrievalItemViewModel> GenerateRetrievalItemViewModelWithoutDRIVMList(List<TransactionItem> combinedRetrievalList)
        {
            List<RetrievalItemViewModel> combinedRetrievalItemVMList = new List<RetrievalItemViewModel>();
            foreach(TransactionItem item in combinedRetrievalList)
            {
                RetrievalItemViewModel itemVM = new RetrievalItemViewModel(item.Item, item);
                combinedRetrievalItemVMList.Add(itemVM);
            }
            return combinedRetrievalItemVMList;
        }
        
        
        //generate the combined retrieval list by item.
        public List<RetrievalItemViewModel> GenerateRetrievalItemViewModel(List<TransactionItem> combinedRetrievalList)
        {
            List<RetrievalItemViewModel> combinedRIVMList = new List<RetrievalItemViewModel>();
            List<RetrievalItemViewModel> rivmList = GenerateRetrievalItemViewModelWithoutDRIVMList(combinedRetrievalList);
            var retrievalListList = unitOfWork.RetrievalListRepository.Get(filter: x => x.Status.ToString() == "Approved");
            List<List<DeptRetrievalItemViewModel>> drivmListList = new List<List<DeptRetrievalItemViewModel>>();
            foreach (RetrievalList rl in retrievalListList)
            {
                List<DeptRetrievalItemViewModel> drivmList = GenerateDeptRetrievalItemListByRetrievalList(rl);
                drivmListList.Add(drivmList);
            }

            foreach (RetrievalItemViewModel rivm in rivmList)
            {
                foreach(List<DeptRetrievalItemViewModel> drivmList in drivmListList)
                {
                    DeptRetrievalItemViewModel item = (DeptRetrievalItemViewModel) drivmList.Where(x => x.transactionItem.Item == rivm.item);
                    rivm.deptRetrievalItems.Add(item);
                }
            }
            foreach(RetrievalItemViewModel rivm in rivmList)
            {
                Debug.WriteLine("Items in Model View: " + rivm.item.Description + " Handover Qty: " + rivm.transactionItem.HandOverQty);
            }
            return rivmList;
        } 


        public List<TransactionItem> GenerateCombinedRetrievalList()
        {
            List<TransactionItem> combinedRetrievalList = new List<TransactionItem>();
            List<TransactionItem> tempTransItems = new List<TransactionItem>();
            var deptList = unitOfWork.DepartmentRepository.Get();
            foreach(Department dept in deptList)
            {
                List<TransactionItem>deptRetrievalList = GenerateDeptRetrievalList(dept.ID);
                if(combinedRetrievalList.Count == 0 && deptRetrievalList.Count > 0)
                {
                    foreach(TransactionItem item in deptRetrievalList)
                    {
                        tempTransItems.Add(item);
                        Debug.WriteLine("GenerateCombinedRetrievalList temptransItems contains: " + tempTransItems.Count);
                    }
                }

                foreach(TransactionItem transItem in deptRetrievalList)
                {
                    bool isExist = false;
                    TransactionItem temp;
                    int index = 0;
                    for (int i = 0; i < combinedRetrievalList.Count; i++)
                    {
                        temp = combinedRetrievalList[i];
                        if (temp.Item.ID.Equals(transItem.Item.ID))
                        {
                            isExist = true;
                            index = i;
                            break;
                        }

                    }
                    if (isExist)
                    {
                        combinedRetrievalList[index].HandOverQty += transItem.HandOverQty;
                        combinedRetrievalList[index].TakeOverQty += transItem.TakeOverQty;
                    }
                    else
                    {
                        combinedRetrievalList.Add(transItem);
                    }
                }
            }
            foreach(TransactionItem item in combinedRetrievalList)
            {
                Debug.WriteLine("Combined Retrieval List contains: " + item.Item.Description + " " + item.HandOverQty);
            }
            return combinedRetrievalList;
        }
        
        public void UpdateRequestionOrderStatus(RequisitionOrder RO)
        {
            RO.Status = (Models.Status)4;
            unitOfWork.RequisitionOrderRepository.Update(RO);
            unitOfWork.Save();
        }
        
        public void UpdateTransactionItemTakeoverQty(TransactionItem item, int  qty)
        {
            item.TakeOverQty = qty;

        }
        
        public List<TransactionItem> GenerateDisbursementList(string deptID)
        {
            var completedRetrievals = unitOfWork.RetrievalListRepository.Get(filter: x => x.Status.ToString() == "Completed" && x.Department.ID==deptID);
            List<TransactionItem> disbursementList = new List<TransactionItem>();
            foreach(RetrievalList rl in completedRetrievals)
            {
                List<TransactionItem> retrievedItemList = (List<TransactionItem>)rl.ItemTransactions;
                foreach(TransactionItem retrievedItem in retrievedItemList)
                {
                    bool isExist = false;
                    TransactionItem temp;
                    int index = 0;
                    for(int i = 0; i<retrievedItemList.Count; i++)
                    {

                    }
                    
                    TransactionItem disbursementItem = new TransactionItem(retrievedItem.TakeOverQty, retrievedItem.TakeOverQty, "Disbursement", retrievedItem.Item);
                    
                }
            }
            return null;
        }
       
    }
}