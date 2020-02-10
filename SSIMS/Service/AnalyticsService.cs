using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;

namespace SSIMS.Service
{
    public class AnalyticsService
    {
        UnitOfWork uow = new UnitOfWork();
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

        public static List<AnalyticsDetailsVM> GroupByItemCode(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.ItemCode)
                .Select(cl => new AnalyticsDetailsVM
                {
                    ItemCode = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of per itemcode
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per itemcode
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per item
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByOrderStaff(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.OrderStaff)
                .Select(cl => new AnalyticsDetailsVM
                {
                    OrderStaff = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per staff
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per staff
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per staff
                }).ToList();

            return result;
        }
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

            List<AnalyticsDetailsVM> result = list.Where(x => x.ItemCode == itemcode).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByOrderStaff(List<AnalyticsDetailsVM> list, string orderstaff)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.OrderStaff == orderstaff).ToList();
            return result ?? null;
        }
    }
}