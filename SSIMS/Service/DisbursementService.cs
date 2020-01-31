using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;
using System.Collections;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.Service
{
    public class DisbursementService
    {
        static DatabaseContext context = new DatabaseContext();
        static RequisitionOrderRepository ROrepo = new RequisitionOrderRepository(context);
        static DepartmentRepository deptRepo = new DepartmentRepository(context);
        static StaffRepository staffRepo = new StaffRepository(context);
        static RetrievalListRepository RLrepo = new RetrievalListRepository(context);
        public List<TransactionItem> GenerateDeptRetrievalList(string deptID)
        {
            //requisitionOrders are approved
            
           
            List<DocumentItem> docItemsArray = new List<DocumentItem>();
            ICollection<TransactionItem> retrievalList;

            //retrieve all RO with status approved
            List<RequisitionOrder> ROList = new List<RequisitionOrder>();
            var approvedRO = ROrepo.Get(includeProperties:"DocumentItems.Item,CreatedByStaff.Department" ,filter: x => x.Status.ToString() == "Approved");

            foreach(RequisitionOrder RO in approvedRO)
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

            //filter by department

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
            Debug.WriteLine("Combined Doc List: " + combinedDocList.Count);
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
                    TransactionItem item = new TransactionItem(doc.Qty, doc.Qty, null, doc.Item);
                    transItemList.Add(item);
                }
            }

            foreach (TransactionItem item in transItemList)
            {
                Debug.WriteLine("TransItem List: " + item.Item.Description + " " + item.HandOverQty);
            }
            return transItemList;
        }

        public void SaveToRetrievalListRepo(string deptID)
        {
            Staff clerk = staffRepo.GetByID(10003);
            Department dept = deptRepo.GetByID(deptID);
            List<TransactionItem> deptRetrievalList = GenerateDeptRetrievalList(deptID);
            RetrievalList retrievalList = new RetrievalList(clerk, dept);
            retrievalList.ItemTransactions = deptRetrievalList;
            Debug.WriteLine("Saving to RLrepo...");
            context.RetrievalLists.Add(retrievalList);
            context.SaveChanges();
        }


        public List<TransactionItem> GenerateCombinedRetrievalList()
        {
            List<TransactionItem> combinedRetrievalList = new List<TransactionItem>();
            List<TransactionItem> tempTransItems = new List<TransactionItem>();
            var deptList = deptRepo.Get();
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
        
        
        
        public List<TransactionItem> GenerateDisbursementList(string deptID)
        {
            
            return null;
        }
       
    }
}