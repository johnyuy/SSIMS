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
using SSIMS.Filters;


namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class AnalyticsController : Controller
    {
        // GET: Analytics
        public ActionResult Index()
        {
            AnalyticsListVM analytics;

            if (Session["Analytics"] == null)
            {
                UnitOfWork uow = new UnitOfWork();
                analytics = new AnalyticsListVM(uow);
                Session["Analytics"] = analytics;
            } else
            {
                analytics = (AnalyticsListVM)Session["Analytics"];
            }


            //generate lists for dropdown




            //List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupByCategory(analytics.SummaryList);
            // Analytics TABLE = new Analytics(categorylist, categorylist2);
            //ViewBag.Summarylist= categorylist;
            //return View("AnalyticsList", categorylist);
            return View();
        }

        public ActionResult Chart(string groupby, string groupval, string filter1="", string value1="", string filter2="", string value2="")
        {
            AnalyticsListVM analytics;

            if (Session["Analytics"] == null)
            {
                UnitOfWork uow = new UnitOfWork();
                analytics = new AnalyticsListVM(uow);
                Session["Analytics"] = analytics;
            }
            else
            {
                analytics = (AnalyticsListVM)Session["Analytics"];
            }




            return null;
        }


        public ActionResult Index2()
        {
            AnalyticsListVM analytics;
            if (Session["Analytics"] == null)
            {
                UnitOfWork uow = new UnitOfWork();
                List<RequisitionOrder> requisitionOrderList = (List<RequisitionOrder>)uow.RequisitionOrderRepository.Get(includeProperties: "CreatedByStaff.Department,DocumentItems.Item");
                analytics = new AnalyticsListVM(uow);
                Session["Analytics"] = analytics;
            }
            else
            {
                analytics = (AnalyticsListVM)Session["Analytics"];
            }

            List<AnalyticsDetailsVM> categorylist2 = AnalyticsService.GroupByDepartment(analytics.SummaryList);
            // Debug.WriteLine("hello :" + categorylist.First().CreatedDate.ToString("yyyy"));
            ViewBag.Summarylist1 = categorylist2;

            return View("AnalyticsList2", categorylist2);
        }
        public ActionResult Chart1()
        {
            UnitOfWork uow = new UnitOfWork();
            AnalyticsListVM analyticsListViewModel = new AnalyticsListVM(uow);
            List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupByCategory(analyticsListViewModel.SummaryList);
             
          // RequisitionSummaryViewModel clipCategory = AnalyticsService.GroupByCategory("Clip", summaryList);


            ArrayList xValue = AnalyticsService.QtyCategoryChart1X(categorylist);
            ArrayList yValue = AnalyticsService.QtyCategoryChart1Y(categorylist);
            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Chart for Quantity and Category")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("bmp");
            return null;
           
        }
        public ActionResult Chart2()
        {
            UnitOfWork uow = new UnitOfWork();
            AnalyticsListVM analyticsListViewModel = new AnalyticsListVM(uow);
            List<AnalyticsDetailsVM> categorylist = AnalyticsService.GroupByDepartment(analyticsListViewModel.SummaryList);

            ArrayList xValue = AnalyticsService.QtyCategoryChart2X(categorylist);
            ArrayList yValue = AnalyticsService.QtyCategoryChart2Y(categorylist);
            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Chart for Quantity and Category")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .Write("bmp");
            return null;

        }

        [HttpGet]
        public JsonResult GetFilter2(string filter1)
        {
            IEnumerable<SelectListItem> filter2List = new List<SelectListItem>() ;

            if (!String.IsNullOrWhiteSpace(filter1))
            {

                filter2List = AnalyticsService.GetFilter2List(filter1);

            }

            return Json(filter2List, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetFilterValues(string filter)
        {
            IEnumerable<SelectListItem> valueslist = new List<SelectListItem>();

            if (!String.IsNullOrWhiteSpace(filter))
            {

                valueslist = AnalyticsService.GetFilterValues(filter);

            }

            return Json(valueslist, JsonRequestBehavior.AllowGet);
        }
    }
    
}