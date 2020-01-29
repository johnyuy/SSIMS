using SSIMS.DAL;
using SSIMS.Database;
using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.Controllers
{
    public class AnalyticsController : Controller
    {
        public static RequisitionOrderRepository RequisitionOrderRepository;
        // GET: Analytics
        public ActionResult Index()
        {
            DatabaseContext context = new DatabaseContext();
            RequisitionOrderRepository = new RequisitionOrderRepository(context);
            // get all requisition order
            List<RequisitionOrder> requisitionOrderList = (List<RequisitionOrder>)RequisitionOrderRepository.Get(includeProperties:"CreatedByStaff.Department");
            foreach (RequisitionOrder ro in requisitionOrderList)
            {
                Debug.WriteLine(ro.CreatedByStaff.Name);
                Debug.WriteLine(ro.CreatedByStaff.Department.DeptName);
                Debug.WriteLine(ro.CreatedDate.ToString("dd/MM/yyyy"));
                Debug.WriteLine(ro.CreatedDate.ToString("MM"));
                foreach (DocumentItem di in ro.DocumentItems)
                {
                    //Debug.WriteLine("\t" + di.Item.ID + " x " + di.Qty);
                }
            }

            return View();
        }
        
    }
}