using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.DAL;
using PagedList;
using SSIMS.Service;
using SSIMS.ViewModels;
using SSIMS.Filters;
using Rotativa;

namespace SSIMS.Controllers
{
    //[AuthenticationFilter]
    //[AuthorizationFilter]
    public class RetrievalController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();
        private DisbursementService ds = new DisbursementService();
        private readonly ILoginService loginService = new LoginService();
        // GET: RetrievalLists
        public ActionResult Index(int? page, string status="InProgress")
        {
            var retrievalList = uow.RetrievalListRepository.Get(includeProperties: "CreatedByStaff, ItemTransactions.Item, Department");
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            switch (status)
            {
                
                case "InProgress":
                    retrievalList = retrievalList.Where(x => x.Status == Models.Status.InProgress).ToList();
                    break;
                case "Completed":
                    retrievalList = retrievalList.Where(x => x.Status == Models.Status.Completed).ToList();
                    break;
                case "All":
                    retrievalList = retrievalList.ToList();
                    break;
                default:
                    retrievalList = retrievalList.Where(x => x.Status == Models.Status.InProgress).ToList();
                    break;
            }
            ViewBag.RetrievalStatus = status;
            return View(retrievalList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Current()
        {

            List<TransactionItem> combinedRL = ds.ViewCombinedRetrievalList();
            var rivm = ds.ViewRetrievalItemViewModel(combinedRL);
            var retrievalLists = db.RetrievalLists.Include(d => d.CreatedByStaff).Include(d => d.Status);

            if (retrievalLists == null)
            {
                return HttpNotFound();
            }

            return View("CurrentRetrieval",rivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Current([Bind(Include = "ROList, rivmlist")] RetrievalVM model)
        {
            UnitOfWork uow = new UnitOfWork();

            List<RequisitionOrder> ROList = model.ROList;
            foreach (RequisitionOrder RO in ROList)
            {
                RequisitionOrder NewRO = uow.RequisitionOrderRepository.GetByID(RO.ID);
                NewRO.Completed(loginService.StaffFromSession);
                uow.RequisitionOrderRepository.Update(NewRO);
                uow.Save();
            }

            List<RetrievalItemViewModel> rivmList = model.rivmlist;
            ds.InsertRetrievalList(rivmList);

            return RedirectToAction("Index");
        }

        public ActionResult Print(int? id)
        {
            UnitOfWork uow = new UnitOfWork();

            RetrievalList retrievalList = uow.RetrievalListRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, ItemTransactions.Item").FirstOrDefault();
            //DisbursementList disbursementList = unitOfWork.DisbursementListRepository.GetByID(id);
            var pdfResult = new ActionAsPdf("Details", new { id = id });

            return pdfResult;
        }

        public ActionResult GenerateDisbursement()
        {
            //authorize user

            List<RetrievalList> retrievalLists = (List<RetrievalList>)uow.RetrievalListRepository.Get(filter: x => x.Status == Models.Status.InProgress, includeProperties: "Department");
            var deptList = retrievalLists.Select(x => x.Department.ID).Distinct();
            //List<Department> deptList = (List<Department>)uow.DepartmentRepository.Get();
            //List<DeptDisbursementViewModel> deptDVMList = new List<DeptDisbursementViewModel>();
            foreach (string dept in deptList)
            {
                //for each retrievallists create adn save disbursement list
                //change retrieval list status to complete
                ds.InsertDisbursementList(dept);
            }


            return RedirectToAction("Current", "Disbursement");
        }

        // Show in progress & completed retrieval
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var retrievalList = (RetrievalList)uow.RetrievalListRepository.Get(filter:x => x.ID == id, includeProperties: "CreatedByStaff, Department, ItemTransactions.Item").FirstOrDefault();
            if (retrievalList == null)
            {
                return HttpNotFound();
            }
            return View(retrievalList);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
