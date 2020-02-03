using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    public class AnalyticsService
    {
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
                output.Add(rvsm.Qty);
            }
            if (output.Count > 0)
                return output;

            return null;
        }

    }
}