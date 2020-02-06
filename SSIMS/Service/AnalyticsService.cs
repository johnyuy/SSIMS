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
        public static ArrayList GetQtyCategoryChart1X(List<RequisitionSummaryViewModel> SummaryList)
        {

            ArrayList output = new ArrayList();
            foreach (RequisitionSummaryViewModel rvsm in SummaryList)
            {
                output.Add(rvsm.Category);
            }
            if (output.Count > 0)
                return output;
            
            return null;
        }
        public static ArrayList GetQtyCategoryChart1Y(List<RequisitionSummaryViewModel> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (RequisitionSummaryViewModel rvsm in SummaryList)
            {
                output.Add(rvsm.Qty);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList GetQtyCategoryChart2X(List<RequisitionSummaryViewModel> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (RequisitionSummaryViewModel rvsm in SummaryList)
            {
                output.Add(rvsm.Department);
            }
            if (output.Count > 0)
                return output;

            return null;
        }

        public static ArrayList GetQtyCategoryChart2Y(List<RequisitionSummaryViewModel> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (RequisitionSummaryViewModel rvsm in SummaryList)
            {
                output.Add(rvsm.count);
            }
            if (output.Count > 0)
                return output;

            return null;
        }

        public static List<RequisitionSummaryViewModel> GroupSummaryListByCategory(List<RequisitionSummaryViewModel> SummaryList)
        {
            if(SummaryList!=null && SummaryList.Count > 0)
            {
                List<RequisitionSummaryViewModel> result = SummaryList
                    .GroupBy(l => l.Category)
                    .Select(cl => new RequisitionSummaryViewModel
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

        public static List<RequisitionSummaryViewModel> GroupSummaryListByDepartment(List<RequisitionSummaryViewModel> SummaryList)
        {
            if (SummaryList != null && SummaryList.Count > 0)
            {
                List<RequisitionSummaryViewModel> result = SummaryList
                    .GroupBy(l => l.Department)
                    .Select(cl => new RequisitionSummaryViewModel
                    {   Qty=0,
                        Category = "",
                        Department = cl.Key,
                        Month = 0,
                        count = cl.Count()
                    }).ToList();

                return result;
            }

            return null;
        }



    }
}