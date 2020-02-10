using SSIMS.DAL;
using SSIMS.Database;
using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.ViewModels;
using System.Collections;
using System.Web.Helpers;
using SSIMS.Service;


namespace SSIMS.Controllers
{
    public class AnalyticsController : Controller
    {
        public static RequisitionOrderRepository RequisitionOrderRepository;
        // GET: Analytics
        public ActionResult Index()
        {
            DatabaseContext context = new DatabaseContext();
            AnalyticsListVM analyticsListViewModel = new AnalyticsListVM();
            RequisitionOrderRepository = new RequisitionOrderRepository(context);
            // get all requisition order
            List<RequisitionOrder> requisitionOrderList = (List<RequisitionOrder>)RequisitionOrderRepository.Get(includeProperties:"CreatedByStaff.Department,DocumentItems.Item");
            /* foreach (RequisitionOrder ro in requisitionOrderList)
             {
                 Debug.WriteLine(ro.CreatedByStaff.Name);
                 Debug.WriteLine(ro.CreatedByStaff.Department.DeptName);
                 Debug.WriteLine(ro.CreatedDate.ToString("dd/MM/yyyy"));
                 Debug.WriteLine(ro.CreatedDate.ToString("MM"));
                 foreach (DocumentItem di in ro.DocumentItems)
                 {
                     Debug.WriteLine("\t" + di.Item.ID + " x " + di.Qty);
                 }
             }*/
            
            List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupSummaryListByCategory(analyticsListViewModel.SummaryList);
            List<AnalyticsDetailsVM> categorylist2 = AnalyticsService.GroupSummaryListByDepartment(analyticsListViewModel.SummaryList);
            // Debug.WriteLine("hello :" + categorylist.First().CreatedDate.ToString("yyyy"));
           // Analytics TABLE = new Analytics(categorylist, categorylist2);
            ViewBag.Summarylist= categorylist;
            
            return View("AnalyticsList", categorylist);
        }

        public ActionResult Index2()
        {
            DatabaseContext context = new DatabaseContext();
            AnalyticsListVM analyticsListViewModel = new AnalyticsListVM();
            RequisitionOrderRepository = new RequisitionOrderRepository(context);
            // get all requisition order
            List<RequisitionOrder> requisitionOrderList = (List<RequisitionOrder>)RequisitionOrderRepository.Get(includeProperties: "CreatedByStaff.Department,DocumentItems.Item");
            List<AnalyticsDetailsVM> categorylist2 = AnalyticsService.GroupSummaryListByDepartment(analyticsListViewModel.SummaryList);
            // Debug.WriteLine("hello :" + categorylist.First().CreatedDate.ToString("yyyy"));
            ViewBag.Summarylist1 = categorylist2;

            return View("AnalyticsList2", categorylist2);
        }
        public ActionResult Chart1()
        {  AnalyticsListVM analyticsListViewModel = new AnalyticsListVM();
            List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupSummaryListByCategory(analyticsListViewModel.SummaryList);
             
          // RequisitionSummaryViewModel clipCategory = AnalyticsService.GroupByCategory("Clip", summaryList);


            ArrayList xValue = AnalyticsService.GetQtyCategoryChart1X(categorylist);
            ArrayList yValue = AnalyticsService.GetQtyCategoryChart1Y(categorylist);
            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Chart for Quantity and Category")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("bmp");
            return null;

        }
        public ActionResult Chart2()
        {
            AnalyticsListVM analyticsListViewModel = new AnalyticsListVM();
            List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupSummaryListByDepartment(analyticsListViewModel.SummaryList);

            ArrayList xValue = AnalyticsService.GetQtyCategoryChart2X(categorylist);
            ArrayList yValue = AnalyticsService.GetQtyCategoryChart2Y(categorylist);
            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Chart for Quantity and Category")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("bmp");
            return null;

        }


    }
    
}