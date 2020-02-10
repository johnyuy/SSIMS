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
        public List<RequisitionOrder> ROList;
        public List<AnalyticsDetailsVM> SummaryList;
        public AnalyticsListVM()
        {
            List<AnalyticsDetailsVM> summaryList = new List<AnalyticsDetailsVM>();
            UnitOfWork unitOfWork = new UnitOfWork();
            Debug.WriteLine(DateTime.Now.Year.ToString());

            var items = unitOfWork.RequisitionOrderRepository
                .Get(includeProperties: "CreatedByStaff.Department, DocumentItems.Item");
            ROList = items.ToList();

            //Debug.WriteLine("Number of items = " + ROList.Count());
            foreach (RequisitionOrder ro in ROList)
            {
                foreach (DocumentItem di in ro.DocumentItems)
                {
                    AnalyticsDetailsVM rvsm = new AnalyticsDetailsVM(di.Qty, di.Item.Category, ro.CreatedDate, ro.CreatedByStaff.Department.ID, di.Item.Description, ro.CreatedByStaff.Name);
                    
                    summaryList.Add(rvsm);
                    
                }
            }
            SummaryList = summaryList;
            
        }
    }
}