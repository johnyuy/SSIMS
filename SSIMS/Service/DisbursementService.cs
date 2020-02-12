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
        ILoginService loginService = new LoginService();
        static Random rnd = new Random();
        NotificationService ns = new NotificationService();

        public static int GenerateOTP()
        {
            int otp = rnd.Next(1000, 9999);
            return otp;
        }

        //returns a list of TransactionIems from approved Request Orders of a given dept
        public List<TransactionItem> GenerateDeptRetrievalList(string deptID, bool toSave)
        {
            //requisitionOrders are approved

            List<DocumentItem> docItemsArray = new List<DocumentItem>();

            //retrieve all RO with status approved
            List<RequisitionOrder> ROList = new List<RequisitionOrder>();
            var approvedRO = (List<RequisitionOrder>)unitOfWork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved, includeProperties: "DocumentItems.Item,CreatedByStaff.Department");
            Debug.WriteLine("Number of approved ROs: " + approvedRO.Count);
            //filter by department
            foreach (RequisitionOrder RO in approvedRO)
            {
                Debug.WriteLine("RO ID: " + RO.ID);
                Debug.WriteLine("RO is created by: " + RO.CreatedByStaff.Name + " from " + RO.CreatedByStaff.Department.ID);
                string deptRO = RO.CreatedByStaff.Department.ID;
                Debug.WriteLine("RO is from dept: " + deptRO);
                if (deptRO.Equals(deptID))
                {

                    if (toSave == true)
                    {
                        Debug.WriteLine("Setting RO status to Completed...");
                        UpdateRequestionOrderStatus(RO, 4); //Set Status to "Completed"
                    }
                    ROList.Add(RO);
                }
                Debug.WriteLine("Number of elements in ROList: " + ROList.Count);
            }

            //get all ROs DI into a combined list
            List<DocumentItem> combinedDocList = new List<DocumentItem>();
            for (int i = 0; i < ROList.Count; i++)
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

            if (transItemList.Count == 0 && combinedDocList.Count > 0)
            {
                TransactionItem item = new TransactionItem(0, 0, null, combinedDocList[0].Item);
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


        //generate the retrieval list item for the model view
        public DeptRetrievalItemViewModel GenerateDeptRetrievalItem(Department d, Item item)
        {

            // find transactional item using dept and item and in progress
            var deptRetrievalList = (RetrievalList)unitOfWork.RetrievalListRepository.Get(includeProperties: "Department, ItemTransactions.Item", filter: x => x.Department.ID == d.ID && x.Status == Models.Status.InProgress);
            TransactionItem tItem = (TransactionItem)deptRetrievalList.ItemTransactions.Where(x => x.Item == item);

            // using found item to construct DeptRetrievalItem
            DeptRetrievalItemViewModel deptRetrievalItem = new DeptRetrievalItemViewModel(d.ID, tItem);

            return deptRetrievalItem;
        }

        //generate the retrieval list item for the model view using Retrieval List as input
        public List<DeptRetrievalItemViewModel> GenerateDeptRetrievalItemListByRetrievalList(RetrievalList retrievalList)
        {
            //List<TransactionItem> itemList = (List<TransactionItem>)retrievalList.ItemTransactions;
            List<DeptRetrievalItemViewModel> drivmList = new List<DeptRetrievalItemViewModel>();
            foreach (TransactionItem item in retrievalList.ItemTransactions)
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
            foreach (TransactionItem item in combinedRetrievalList)
            {
                RetrievalItemViewModel itemVM = new RetrievalItemViewModel(item.Item, item);
                combinedRetrievalItemVMList.Add(itemVM);
            }
            return combinedRetrievalItemVMList;
        }


        //generate the combined retrieval list by item, adding in the DRIVM List here.
        public RetrievalVM ViewRetrievalItemViewModel(List<TransactionItem> combinedRetrievalList)
        {
            List<RetrievalItemViewModel> combinedRIVMList = new List<RetrievalItemViewModel>();
            List<RetrievalItemViewModel> rivmList = GenerateRetrievalItemViewModelWithoutDRIVMList(combinedRetrievalList);
            List<List<DeptRetrievalItemViewModel>> drivmListList = new List<List<DeptRetrievalItemViewModel>>();
            var deptList = unitOfWork.DepartmentRepository.Get();
            foreach (Department dept in deptList)
            {
                RetrievalList deptRL = ViewDeptRetrievalList(dept.ID);
                List<DeptRetrievalItemViewModel> drivmList = GenerateDeptRetrievalItemListByRetrievalList(deptRL);
                drivmListList.Add(drivmList);
            }
            foreach (RetrievalItemViewModel rivm in rivmList)
            {
                foreach (List<DeptRetrievalItemViewModel> drivmList in drivmListList)
                {
                    foreach (DeptRetrievalItemViewModel drivm in drivmList)
                    {
                        if (drivm.transactionItem.Item.Equals(rivm.item))
                        {
                            Debug.WriteLine("Adding into RIVM: " + drivm.deptID + " " + drivm.transactionItem.Item.Description + " " + drivm.transactionItem.HandOverQty);
                            rivm.deptRetrievalItems.Add(drivm);
                        }
                    }
                }
            }

            List<RequisitionOrder> requisitionOrderList = (List<RequisitionOrder>)unitOfWork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved);

            Debug.WriteLine("Number of Approved ROs: " + requisitionOrderList.Count);
            RetrievalVM retrivalVM = new RetrievalVM();
            retrivalVM.ROList = requisitionOrderList;
            retrivalVM.rivmlist = rivmList;
            return retrivalVM;

        }

        public RetrievalList ViewDeptRetrievalList(string deptID)
        {
            Staff clerk = unitOfWork.StaffRepository.GetByID(10003);
            var dept = unitOfWork.DepartmentRepository.GetByID(deptID);
            List<TransactionItem> deptRetrievalList = GenerateDeptRetrievalList(deptID, false);
            RetrievalList retrievalList = new RetrievalList(clerk, dept);
            retrievalList.ItemTransactions = deptRetrievalList;
            return retrievalList;
        }

        public List<TransactionItem> ViewCombinedRetrievalList()
        {
            Debug.WriteLine("In ViewCombinedRetrievalList");
            List<TransactionItem> combinedRetrievalList = new List<TransactionItem>();
            List<TransactionItem> tempTransItems = new List<TransactionItem>();
            var deptList = unitOfWork.DepartmentRepository.Get();
            foreach (Department dept in deptList)
            {
                List<TransactionItem> deptRetrievalList = GenerateDeptRetrievalList(dept.ID, false);
                if (combinedRetrievalList.Count == 0 && deptRetrievalList.Count > 0)
                {
                    foreach (TransactionItem item in deptRetrievalList)
                    {
                        tempTransItems.Add(item);
                        Debug.WriteLine("GenerateCombinedRetrievalList temptransItems contains: " + tempTransItems.Count);
                    }
                }

                foreach (TransactionItem transItem in deptRetrievalList)
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
            foreach (TransactionItem item in combinedRetrievalList)
            {
                Debug.WriteLine("Combined Retrieval List contains: " + item.Item.Description + " " + item.HandOverQty);
            }
            Debug.WriteLine("Exiting ViewCombinedRetrievalList");
            return combinedRetrievalList;
        }

        public void UpdateRequestionOrderStatus(RequisitionOrder RO, int status)
        {
            RO.Status = (Models.Status)status;
            unitOfWork.RequisitionOrderRepository.Update(RO);
            unitOfWork.Save();
        }


        //Condensing all the retrieval lists from that dept that are InProgress (i.e. not disbursed yet) into a combined retrieval list with unique items
        //Changing the status of the retrieval list to Completed. 
        public bool InsertDisbursementList(string deptID)
        {
            UnitOfWork uow = new UnitOfWork();
            bool isSuccessful = true;
            var completedRetrievals = uow.RetrievalListRepository.Get(filter: x => x.Status == Models.Status.InProgress && x.Department.ID == deptID, includeProperties: "ItemTransactions.Item, Department").ToList();

            List<TransactionItem> combinedTransItemList = new List<TransactionItem>();
            //List<TransactionItem> tempTransItems = new List<TransactionItem>();


            foreach (RetrievalList rlist in completedRetrievals)
            {
                rlist.Completed(loginService.StaffFromSession);
                foreach (TransactionItem transItem in rlist.ItemTransactions)
                {
                    combinedTransItemList.Add(transItem);
                }
            }
            
            var tempTransItems = combinedTransItemList.Select(x => x.Item.ID).Distinct();

            List<TransactionItem> uniqueTransItemList = new List<TransactionItem>();
            foreach (string itemID in tempTransItems)
            {
                TransactionItem i = new TransactionItem(itemID, uow);
                uniqueTransItemList.Add(i);
            }

            foreach (TransactionItem transItem in uniqueTransItemList)
            {
                foreach (TransactionItem i in combinedTransItemList)
                {
                    if (i.Item.ID == transItem.Item.ID)
                    {
                        transItem.HandOverQty = transItem.HandOverQty + i.TakeOverQty;
                        transItem.TakeOverQty = transItem.TakeOverQty + i.TakeOverQty;
                        transItem.Reason = "Disbursement";
                    }
                }
            }

            if(uniqueTransItemList == null || uniqueTransItemList.Count == 0)
            {
                return isSuccessful = false;
            }
            else
            {
                Department dept = uow.DepartmentRepository.GetByID(deptID);
                DisbursementList deptDL = new DisbursementList(uniqueTransItemList, dept);
                Staff clerk = loginService.StaffFromSession;
                deptDL.CreatedByStaff = clerk;
                uow.DisbursementListRepository.Insert(deptDL);
                uow.Save();
                bool isSent = SendOTPNotification(deptDL, uow);

            }
            return isSuccessful;

        } 

        public bool SendOTPNotification(DisbursementList disbursementList, UnitOfWork uow)
        {
            bool isSent = true;
            Staff deptRep = uow.StaffRepository.Get(filter: x => x.DepartmentID == disbursementList.Department.ID && x.StaffRole == "DeptRep", includeProperties:"Department.CollectionPoint").FirstOrDefault();
            if(deptRep == null)
            {
                return isSent = false;
            }

            string collectionTime = deptRep.Department.CollectionPoint.Time.ToString("hh:mm tt");
            string RepEmail = "nataliehsm@gmail.com";
            string subject = "Verification Code";
            string msg = "Stationary Disbursement " + disbursementList.ID.ToString($"DL{ 0:1000000}") + " for " + disbursementList.Department.DeptName + " Department is ready for collection." +
                "\nCollection Point: " + deptRep.Department.CollectionPoint.Location +
                "\nTime: " + collectionTime +
                "\nPlease provide OTP: " + disbursementList.OTP + " to our clerk during collection. \nThis is a System Generated Message, do not reply.";

            ns.SendEmail(RepEmail, subject, msg);
            return isSent; 
        }

        public bool VerifyOTP (int DisbursementID, string otpEntered, UnitOfWork uow)
        {
            DisbursementList dl = uow.DisbursementListRepository.GetByID(DisbursementID);
            if (dl == null) return false;
            if (dl.OTP.ToString().Equals(otpEntered)) return true;
            return false;
        }

        public void InsertRetrievalList(List<RetrievalItemViewModel> rivmList)
        {
            var deptRetrievalItemViewModel = new List<DeptRetrievalItemViewModel>();

            // consolidate all dept retrival item to a single list 
            foreach (RetrievalItemViewModel rivm in rivmList)
            {
                // find item from database
                Debug.WriteLine("rivm items are: " + rivm.item.Description);
                foreach (DeptRetrievalItemViewModel drvm in rivm.deptRetrievalItems)
                {
                    //construct full transactionitem with referencee to db 
                    
                    Debug.WriteLine("drvm items are: " + drvm.transactionItem.Item.ID);
                    deptRetrievalItemViewModel.Add(drvm);
                }
            }
            var temp = from x in deptRetrievalItemViewModel select new { x.deptID };

            var deptIdList = temp.GroupBy(i => i.deptID).Select(y => y.First());
            //Unique Departments with in deptRetrievalItemViewModel
            List<string> deptids = new List<string>();

            foreach (var x in deptIdList)
            {
                deptids.Add(x.deptID);
            }

            foreach (string deptid in deptids)
            {
                RetrievalList rl = new RetrievalList(loginService.StaffFromSession, unitOfWork.DepartmentRepository.GetByID(deptid));
                List<TransactionItem> transactionItems = new List<TransactionItem>();
                foreach (DeptRetrievalItemViewModel drvm in deptRetrievalItemViewModel)
                {
                    if (drvm.deptID == deptid)
                    {
                        TransactionItem transItem = new TransactionItem(drvm,unitOfWork);
                        transactionItems.Add(transItem);
                    }
                }
                rl.ItemTransactions = transactionItems;

                if (unitOfWork.StockCardEntryRepository.ProcessRetrivalListCompletion(rl))
                {
                    rl.InProgress();
                    unitOfWork.RetrievalListRepository.Insert(rl);
                    unitOfWork.Save();
                    Debug.WriteLine("Retrieval List is inserted into database successfully.");
                    InventoryService inventoryService = new InventoryService();
                    foreach(TransactionItem ti in rl.ItemTransactions)
                    {
                        inventoryService.UpdateInStoreQty(ti.Item.ID);
                    }
                    
                }

                
            }
        }
    }
}