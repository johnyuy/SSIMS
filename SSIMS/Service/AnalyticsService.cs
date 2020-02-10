using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;
using System.Web.Mvc;

namespace SSIMS.Service
{
    public class AnalyticsService
    {
        UnitOfWork uow = new UnitOfWork();
        
        public static void GetAnalyticsData(string group, string filter1, string filter2, string value1, string value2)
        {
            List<AnalyticsDetailsVM> data;

            if (HttpContext.Current.Session["AData"] == null)
            {
                AnalyticsListVM analytics = (AnalyticsListVM)HttpContext.Current.Session["Analytics"];
                data = analytics.SummaryList;
                data = ApplyFilter(data, filter1, value1);
                data = ApplyFilter(data, filter2, value2);
                data = ApplyGroup(data, group);
                HttpContext.Current.Session["AData"] = data;
            }
        }



        public static List<AnalyticsDetailsVM> ApplyFilter(List<AnalyticsDetailsVM> data, string filter, string value)
        {
            if (data == null || data.Count == 0) return null;
            if (filter == "" || value == "") return data;

            List<AnalyticsDetailsVM> output = null;
            if (filter == "Department") { output = FilterByDepartment(data, value); }
            else if (filter == "Category") { output = FilterByCategory(data, value); }
            else if (filter == "Item") { output = FilterByItem(data, value); }
            else if (filter == "Staff") { output = FilterByStaff(data, value); }
            else if (filter == "Month") {
                if (int.TryParse(value, out int month))
                    output = FilterByMonth(data, month);
                else
                    return null;
            }
            else if (filter == "Year")
            {
                if (int.TryParse(value, out int year))
                    output = FilterByYear(data, year);
                else
                    return null;
            }

            return output ?? null;
        }
        public static List<AnalyticsDetailsVM> ApplyGroup(List<AnalyticsDetailsVM> data, string group)
        {
            if (data == null || data.Count == 0) return null;
            if (group == "") return data;
            List<AnalyticsDetailsVM> output = null;
            if(group == "Department") { output = GroupByDepartment(data); }
            else if(group == "Category") { output = GroupByCategory(data); }
            else if(group == "Item") { output = GroupByItem(data); }
            else if(group == "Staff") { output = GroupByStaff(data); }
            else if(group == "Month") { output = GroupByMonth(data); }
            else if(group == "Year") { output = GroupByYear(data); }


            return output ?? null; ;
        }
        //For numerical fields like qty, count or cost
        public static ArrayList XAxis(List<AnalyticsDetailsVM> data, string group)
        {
            if (data == null || data.Count() == 0 || group=="") return null;
            ArrayList output = new ArrayList();
            foreach(AnalyticsDetailsVM item in data)
                output.Add(item.GetType().GetProperty(group).GetValue(item, null));
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisCost(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Cost);
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisQty(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Qty);
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisCount(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Count);
            return output.Count > 0 ? output : null;
        }

        //FILTER Methods used first before GROUP Methods

        public static List<AnalyticsDetailsVM> FilterByDepartment(List<AnalyticsDetailsVM> list, string deptID)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Department == deptID).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByCategory(List<AnalyticsDetailsVM> list, string category)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Category == category).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByMonth(List<AnalyticsDetailsVM> list, int month)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Month == month).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByYear(List<AnalyticsDetailsVM> list, int year)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Year == year).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByItem(List<AnalyticsDetailsVM> list, string itemcode)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Item == itemcode).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByStaff(List<AnalyticsDetailsVM> list, string orderstaff)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Staff == orderstaff).ToList();
            return result ?? null;
        }

        //GROUP Methods are Aggregate Functions
        public static List<AnalyticsDetailsVM> GroupByCategory(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Category)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Category = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty requested per category
                    Count = cl.Sum(c=> c.Count),    //Number of times an item from this category requested (w/o qty)
                    Cost = cl.Sum(c=> c.Cost),      //Cost of all items requested per category
                }).ToList();

            return result??null;
        }

        public static List<AnalyticsDetailsVM> GroupByDepartment(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Department)
                .Select(cl => new AnalyticsDetailsVM
                {  
                    Department = cl.Key,
                    Qty = cl.Sum(c=>c.Qty),         //total qty of items ordered per department
                    Count = cl.Sum(c=> c.Count),    //Number of item requests made per department (w/o qty)
                    Cost = cl.Sum(c=>c.Cost)        //Total default tender chargeback per department
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByMonth(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Month)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Month = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per month (need to check year filter)
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per month (w/o qty)
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per month (need to check year filter)
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByYear(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Year)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Year = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per year
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per year (w/o qty)
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per year
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByItem(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Item)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Item = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of per itemcode
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per itemcode
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per item
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByStaff(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Staff)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Staff = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per staff
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per staff
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per staff
                }).ToList();

            return result;
        }

        public static IEnumerable<SelectListItem> GetFilter2List(string filter1){
            List<SelectListItem> output = new List<SelectListItem>();
            
            output.Add(new SelectListItem { Value = "Category", Text = "Category" }); //1
            output.Add(new SelectListItem { Value = "Department", Text = "Department" }); //2
            output.Add(new SelectListItem { Value = "Year", Text = "Year" }); //3
            output.Add(new SelectListItem { Value = "Month", Text = "Month" }); //4
            output.Add(new SelectListItem { Value = "Item", Text = "Item" }); //5
            output.Add(new SelectListItem { Value = "Staff", Text = "Staff" }); //6
            if (filter1 == "Category" || filter1 == "Item") { output.RemoveAt(0); output.RemoveAt(3); }
            else if (filter1 == "Department" || filter1 == "Staff") { output.RemoveAt(1); output.RemoveAt(4); }
            else if (filter1 == "Year") { output.RemoveAt(2); }
            else if (filter1 == "Month") { output.RemoveAt(3); }
            return new SelectList(output, "Value", "Text");
        }
        public static IEnumerable<SelectListItem> GetFilterValues(string filter1)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SelectListItem> output = new List<SelectListItem>();
            if (filter1 == "Category") { output = uow.ItemRepository.GetCategories().ToList(); }
            else if (filter1 == "Item") { output = uow.ItemRepository.GetAllDescriptions().ToList(); }
            else if (filter1 == "Year") { output = GetYearValues().ToList(); }
            else if (filter1 == "Month") { output = GetMonthValues().ToList(); }
            else if (filter1 == "Department") { output = uow.DepartmentRepository.GetAllDepartmentIDs().ToList(); }
            else if (filter1 == "Staff") { output = uow.StaffRepository.GetAllStaffNames().ToList(); }

            return output;
        }
        public static IEnumerable<SelectListItem> GetYearValues()
        {
            List<SelectListItem> output = new List<SelectListItem>();
            int startyear = 2018;
            int years = DateTime.Now.Year - startyear;
            for(int i = 0; i <= years; i++)
            {
                output.Add(new SelectListItem
                {
                    Value = (DateTime.Now.Year - i).ToString(),
                    Text = (DateTime.Now.Year - i).ToString()
                });
            }
            var categorytip = new SelectListItem()
            {
                Value = null,
                Text = "- select year -"
            };
            output.Insert(0, categorytip);
            return new SelectList(output, "Value", "Text");
        }
        public static IEnumerable<SelectListItem> GetMonthValues()
        {
            List<SelectListItem> output = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                string month ="";
                if(i == 1) { month = "January"; }
                if(i == 2) { month = "February"; }
                if(i == 3) { month = "March"; }
                if(i == 4) { month = "April"; }
                if(i == 5) { month = "May"; }
                if(i == 6) { month = "June"; }
                if(i == 7) { month = "July"; }
                if(i == 8) { month = "August"; }
                if(i == 9) { month = "September"; }
                if(i == 10) { month = "October"; }
                if(i == 11) { month = "November"; }
                if(i == 12) { month = "December"; }
                output.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text =  month
                });
            }
            var categorytip = new SelectListItem()
            {
                Value = null,
                Text = "- select month -"
            };
            output.Insert(0, categorytip);
            return new SelectList(output, "Value", "Text");
        }

        public static ArrayList QtyCategoryChart1X(List<AnalyticsDetailsVM> SummaryList)
        {

            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Category);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart1Y(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Qty);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart2X(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Department);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart2Y(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Count);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
    }
}