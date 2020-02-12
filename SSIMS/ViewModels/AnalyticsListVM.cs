using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.ViewModels;
using SSIMS.Models;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using SSIMS.Service;

namespace SSIMS.ViewModels
{
    public class AnalyticsListVM
    {
        public List<RequisitionOrder> ROList { get; set; }
        public List<AnalyticsDetailsVM> ROSummaryList { get; set; }
        public List<DisbursementList> DLList { get; set; }
        public List<AnalyticsDetailsVM> DLSummaryList { get; set; }

        public AnalyticsListVM(UnitOfWork uow)
        {

            List<AnalyticsDetailsVM> rosummaryList = new List<AnalyticsDetailsVM>();
            UnitOfWork unitOfWork = new UnitOfWork();

            var items = unitOfWork.RequisitionOrderRepository
                .Get(includeProperties: "CreatedByStaff.Department, DocumentItems.Item");
            ROList = items.ToList();

            //Debug.WriteLine("Number of items = " + ROList.Count());
            foreach (RequisitionOrder ro in ROList)
            {
                foreach (DocumentItem di in ro.DocumentItems)
                {
                    AnalyticsDetailsVM rvsm = new AnalyticsDetailsVM(di.Qty, di.Item.Category, ro.CreatedDate, ro.CreatedByStaff.Department.ID, di.Item.Description, ro.CreatedByStaff.Name, uow.TenderRepository.GetItemPriceDefaultSupplier(di.Item.ID));
                    
                    rosummaryList.Add(rvsm);
                    
                }
            }
            ROSummaryList = rosummaryList;

            /*var disbitems = unitOfWork.DisbursementListRepository.
                Get(filter:x=>x.Status == SSIMS.Models.Status.Completed, includeProperties: "CreatedByStaff.Department, ItemTransactions.Item");
            DLList = disbitems.ToList();

            List<AnalyticsDetailsVM> dlsummarylist = new List<AnalyticsDetailsVM>();

            foreach (DisbursementList dl in DLList)
            {
                foreach (TransactionItem ti in dl.ItemTransactions)
                {
                    AnalyticsDetailsVM rvsm = new AnalyticsDetailsVM(ti.TakeOverQty, ti.Item.Category, dl.CreatedDate, dl.Department.ID, ti.Item.Description, dl.RepliedByStaff.Name, uow.TenderRepository.GetItemPriceDefaultSupplier(ti.Item.ID));

                    dlsummarylist.Add(rvsm);

                }
            }
            DLSummaryList = dlsummarylist;*/
        }
    }
}