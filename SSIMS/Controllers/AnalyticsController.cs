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
        public ActionResult Index()
        {
            return RedirectToAction("Requisitions");
        }

        // GET: Analytics
        public ActionResult Requisitions()
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
            Session["Data"] = analytics.ROSummaryList;
            ViewBag.Mode = "Requisition Orders";

            return View("Index");
        }

        public ActionResult Disbursements()
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

            Session["Data"] = analytics.DLSummaryList;
            ViewBag.Mode = "Completed Disbursements";
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(string group, string filter1 = "None", string filter2 = "", string value1 = "", string value2 = "")
        {
            Debug.WriteLine("group by: " + group + ", " + filter1 + " = " + value1 + ", " + filter2 + " = " + value2);

            Session["AGroup"] = group == "" ? "Category" : group;
            Session["AFilter1"] = filter1;
            Session["AFilter2"] = filter2;
            Session["AValue1"] = value1;
            Session["AValue2"] = value2;

            return null;
        }

        public ActionResult GenerateChartQty2(string t)
        {
            Debug.WriteLine("Called");
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();

            List<AnalyticsDetailsVM> data = (List<AnalyticsDetailsVM>)Session["Data"];

            data = AnalyticsService.ApplyFilter(data, filter1, value1);
            data = AnalyticsService.ApplyFilter(data, filter2, value2);
            data = AnalyticsService.ApplyGroup(data, group);

            ArrayList yValueQty = AnalyticsService.YAxisQty(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }

            int w = xValue.Count * 30;
            new Chart(width: w, height: 400, theme: ChartTheme.Vanilla)
                .AddTitle("Item Quantity" + titlesub + " by " + group)
                .AddSeries("Qty1", chartType: "Line", xValue: xValue, yValues: yValueQty)
                .Write();
            return null;
        }

        public ActionResult GenerateChartCost(string t)
        {
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();

            List<AnalyticsDetailsVM> data = (List<AnalyticsDetailsVM>)Session["Data"];

            data = AnalyticsService.ApplyFilter(data, filter1, value1);
            data = AnalyticsService.ApplyFilter(data, filter2, value2);
            data = AnalyticsService.ApplyGroup(data, group);


            ArrayList yValueCost = AnalyticsService.YAxisCost(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }
            int w = xValue.Count * 20 + 500;
            string charttype = "Column";
            new Chart(width: w, height: 400, themePath: "~/Resources/ChartCost.xml")
                .AddTitle("Cost($)" + titlesub + " by " + group)
                .AddSeries(name: "Cost1", chartType: charttype, xValue: xValue, yValues: yValueCost)
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

            List<AnalyticsDetailsVM> data = (List<AnalyticsDetailsVM>)Session["Data"];

            data = AnalyticsService.ApplyFilter(data, filter1, value1);
            data = AnalyticsService.ApplyFilter(data, filter2, value2);
            data = AnalyticsService.ApplyGroup(data, group);

            ArrayList yValueCount = AnalyticsService.YAxisCount(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }
            int w = xValue.Count * 20 + 500;
            string charttype = "Column";
            new Chart(width: w, height: 400, themePath: "~/Resources/ChartCount.xml")
                .AddTitle("Volume" + titlesub + " by " + group)
                .AddSeries("Count1", chartType: charttype, xValue: xValue, yValues: yValueCount)
                .Write("bmp");

            return null;
        }

        [HttpGet]
        public JsonResult GetFilter2(string filter1)
        {
            IEnumerable<SelectListItem> filter2List = new List<SelectListItem>();

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
            Session["Data"] = AnalyticsService.GetAnalyticsSampleData();
            return null;
        }

        [HttpGet]
        public ActionResult EndSample()
        {

            Session["Analytics"] = null;
            return RedirectToAction("Requisition");
        }

        public ActionResult Chargeback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Chargeback(string year, string month, string category)
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
            //List<AnalyticsDetailsVM> data = analytics.DLSummaryList;
            List<AnalyticsDetailsVM> data = (List<AnalyticsDetailsVM>)Session["Data"];
            string result = year;
            //filter by year
            data = AnalyticsService.ApplyFilter(data, "Year", year);

            if (month != "all")
            {
                result = int.Parse(month).ToString($"{0:00}") + "/" + result;
                data = AnalyticsService.ApplyFilter(data, "Month", month);
            }
            if (category != "all")
            {
                result = " Category " + category + " in " + result;
                data = AnalyticsService.ApplyFilter(data, "Category", category);
            }
            data = AnalyticsService.ApplyGroup(data, "Department");
            ViewBag.Result = result;
            return View(data);
        }


        public ActionResult GenerateChartQty(string t)
        {
            Debug.WriteLine("Called");
            string group = Session["AGroup"] == null ? "Category" : Session["AGroup"].ToString();
            string filter1 = Session["AFilter1"] == null ? "" : Session["AFilter1"].ToString();
            string filter2 = Session["AFilter2"] == null ? "" : Session["AFilter2"].ToString();
            string value1 = Session["AValue1"] == null ? "" : Session["AValue1"].ToString();
            string value2 = Session["AValue2"] == null ? "" : Session["AValue2"].ToString();

            List<AnalyticsDetailsVM> data = (List<AnalyticsDetailsVM>)Session["Data"];

            data = AnalyticsService.ApplyFilter(data, filter1, value1);
            data = AnalyticsService.ApplyFilter(data, filter2, value2);
            data = AnalyticsService.ApplyGroup(data, group);

            ArrayList yValueQty = AnalyticsService.YAxisQty(data);
            ArrayList xValue = AnalyticsService.XAxis(data, group);

            string titlesub = "";
            if (value1 != "")
            {
                titlesub += " for " + value1;
                if (value2 != "")
                    titlesub += " - " + value2;
            }

            int w = xValue.Count * 20 + 500;
            string charttype = "Column";
            new Chart(width: w, height: 400, themePath: "~/Resources/ChartQty.xml")
                .AddTitle("Item Quantity" + titlesub + " by " + group)
                 .AddSeries("Qty1", chartType: charttype, xValue: xValue, yValues: yValueQty)
                 .Write("bmp");

            return null;

        }
    }

}

