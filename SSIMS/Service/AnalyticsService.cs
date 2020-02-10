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
        public static ArrayList GetQtyCategoryChart1X(List<AnalyticsDetailsVM> SummaryList)
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
        public static ArrayList GetQtyCategoryChart1Y(List<AnalyticsDetailsVM> SummaryList)
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
        public static ArrayList GetQtyCategoryChart2X(List<AnalyticsDetailsVM> SummaryList)
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

        public static ArrayList GetQtyCategoryChart2Y(List<AnalyticsDetailsVM> SummaryList)
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

        public static List<AnalyticsDetailsVM> GroupSummaryListByCategory(List<AnalyticsDetailsVM> SummaryList)
        {
            if(SummaryList!=null && SummaryList.Count > 0)
            {
                List<AnalyticsDetailsVM> result = SummaryList
                    .GroupBy(l => l.Category)
                    .Select(cl => new AnalyticsDetailsVM
                    {
                        Category = cl.Key,
                        Qty = cl.Sum(c => c.Qty),
                        Department = null,
                        Month = 0
                    }).ToList();

                return result;
            }

            return null;
        }

        public static List<AnalyticsDetailsVM> GroupSummaryListByDepartment(List<AnalyticsDetailsVM> SummaryList)
        {
            if (SummaryList != null && SummaryList.Count > 0)
            {
                List<AnalyticsDetailsVM> result = SummaryList
                    .GroupBy(l => l.Department)
                    .Select(cl => new AnalyticsDetailsVM
                    {   Qty=0,
                        Category = "",
                        Department = cl.Key,
                        Month = 0,
                        Count = cl.Count()
                    }).ToList();

                return result;
            }

            return null;
        }



    }
}