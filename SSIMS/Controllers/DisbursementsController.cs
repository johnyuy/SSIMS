using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.Service;
using SSIMS.DAL;
using SSIMS.ViewModels;
using SSIMS.Filters;
using System.Diagnostics;

namespace SSIMS.Controllers
{
    public class DisbursementsController : Controller
    {
        DisbursementsService DisbursementsService = new DisbursementsService();
        
        public ActionResult Index()
        {

            return RedirectToAction("ViewRetrievalLists");
        }

        public ActionResult ViewRetrievalLists(string disburse = "")
        {
            //query string parameter determines if the action is for preview or actual transaction
            bool toDisburse = disburse == "true" ? true : false;
            List<RetrievalListVM> FullRetrievalList  = DisbursementsService.GenerateFullRetrievalList(toDisburse);

            foreach(RetrievalListVM RetrievalListVM in FullRetrievalList)
            {
                Debug.WriteLine("\nRetrievalList for " + RetrievalListVM.Department + ":");
                foreach(RetrievalListItemVM itemVM in RetrievalListVM.DepartmentRetrievalList)
                    Debug.WriteLine("\t" + itemVM.ItemID + " x " + itemVM.HandoverQty);
            }

            //Send model to View
            return View("Retrieval", FullRetrievalList);
        }
    }
}