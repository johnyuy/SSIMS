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

            return View();
        }


        [HttpPost]
        public ActionResult Index(string group,string filter1="None", string filter2 ="",  string value1="", string value2="")
        {
            Debug.WriteLine("group by: " + group + ", " + filter1 + " = " + value1 + ", " + filter2 + " = " + value2);

            Session["AGroup"] = group==""?"Category":group;
            Session["AFilter1"] = filter1;
            Session["AFilter2"] = filter2;
            Session["AValue1"] = value1;
            Session["AValue2"] = value2;

            return null;
            //return RedirectToAction("Index");
        }

        //RIGHT NOW EACH CHART ONLY HAS REQUISITION/CAN HAVE A SERIES FOR ACTUAL DISBURSED
        public ActionResult GenerateChartQty(string t)
        {
            Debug.WriteLine("Called");
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();
            List<AnalyticsDetailsVM> data;
            if (Session["AData"] == null)
            {
                //AnalyticsService.GetAnalyticsData(group, filter1, filter2, value1, value2);
                AnalyticsListVM analytics = (AnalyticsListVM)Session["Analytics"];
                data = analytics.SummaryList;
                data = AnalyticsService.ApplyFilter(data, filter1, value1);
                data = AnalyticsService.ApplyFilter(data, filter2, value2);
                data = AnalyticsService.ApplyGroup(data, group);
            } else
            {
                data = (List<AnalyticsDetailsVM>)Session["AData"];
            }
            ArrayList yValueQty = AnalyticsService.YAxisQty(data);
            ArrayList xValue = AnalyticsService.XAxis(data,group);

            string titlesub = "";
            if(value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2; 
            }

            new Chart(width: 1200, height: 400, theme: ChartTheme.Vanilla)
                .AddTitle("Item Quantity" + titlesub + " by " + group)
                .AddSeries("Qty1", chartType: "Column", xValue: xValue, yValues: yValueQty)
                .Write("bmp");
            return null;
        }

        public ActionResult GenerateChartCost(string t)
        {
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();
            List<AnalyticsDetailsVM> data;
            if (Session["AData"] == null)
            {
                //AnalyticsService.GetAnalyticsData(group, filter1, filter2, value1, value2);
                AnalyticsListVM analytics = (AnalyticsListVM)Session["Analytics"];
                data = analytics.SummaryList;
                data = AnalyticsService.ApplyFilter(data, filter1, value1);
                data = AnalyticsService.ApplyFilter(data, filter2, value2);
                data = AnalyticsService.ApplyGroup(data, group);
            }
            else
            {
                data = (List<AnalyticsDetailsVM>)Session["AData"];
            }
            ArrayList yValueCost = AnalyticsService.YAxisCost(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }

            new Chart(width: 1200, height: 400, ChartTheme.Vanilla)
                .AddTitle("Requisition Cost($)" + titlesub + " by " + group)
                .AddSeries(name: "Cost1", chartType: "Column", xValue: xValue, yValues: yValueCost)
                .Write("bmp");
            return null;
        }

        public ActionResult GenerateChartCount(string t)
        {
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();
            List<AnalyticsDetailsVM> data;
            if (Session["AData"] == null)
            {
                //AnalyticsService.GetAnalyticsData(group, filter1, filter2, value1, value2);
                AnalyticsListVM analytics = (AnalyticsListVM)Session["Analytics"];
                data = analytics.SummaryList;
                data = AnalyticsService.ApplyFilter(data, filter1, value1);
                data = AnalyticsService.ApplyFilter(data, filter2, value2);
                data = AnalyticsService.ApplyGroup(data, group);
            }
            else
            {
                data = (List<AnalyticsDetailsVM>)Session["AData"];
            }
            ArrayList yValueCount = AnalyticsService.YAxisCount(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }

            new Chart(width: 1200, height: 400, theme: ChartTheme.Vanilla)
                .AddTitle("Requisition Volume" + titlesub + " by " + group)
                .AddSeries("Count1", chartType: "Column", xValue: xValue, yValues: yValueCount)
                .Write("bmp");
            
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
            new Chart(width: 1000, height: 400, theme: ChartTheme.Green)
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


        [HttpGet]
        public JsonResult RunSample()
        {
            AnalyticsListVM model = (AnalyticsListVM)Session["Analytics"];
            model.SummaryList = AnalyticsService.GetAnalyticsSampleData();
            Session["Analytics"] = model;
            return null;
        }

        [HttpGet]
        public JsonResult EndSample()
        {
            UnitOfWork uow = new UnitOfWork();
            AnalyticsListVM analytics = new AnalyticsListVM(uow);
            Session["Analytics"] = analytics;
            return null;
        }
    }
    
}