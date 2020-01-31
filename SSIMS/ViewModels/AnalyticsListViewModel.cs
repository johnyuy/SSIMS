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

namespace SSIMS.ViewModels
{
    public class AnalyticsListViewModel
    {
        public List<RequisitionOrder> ROList;
        public List<RequisitionSummaryViewModel> SummaryList;
        public AnalyticsListViewModel()
        {
            List<RequisitionSummaryViewModel> summaryList = new List<RequisitionSummaryViewModel>();
            UnitOfWork unitOfWork = new UnitOfWork();
            Debug.WriteLine(DateTime.Now.Year.ToString());
            var items = unitOfWork.RequisitionOrderRepository.Get(includeProperties: "CreatedByStaff.Department, DocumentItems.Item");
            ROList = items.ToList();
            Debug.WriteLine("Number of items = " + ROList.Count());
            foreach (RequisitionOrder ro in ROList)
            {
                foreach (DocumentItem di in ro.DocumentItems)
                {
                    RequisitionSummaryViewModel rvsm = new RequisitionSummaryViewModel(di.Qty, di.Item.Category, ro.CreatedDate, ro.CreatedByStaff.Department.ID);
                    summaryList.Add(rvsm);
                }
            }
            SummaryList = summaryList;
        }
    }
}