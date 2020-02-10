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
using PagedList;
using Rotativa;

namespace SSIMS.Controllers
{
    //[AuthenticationFilter]
   //[AuthorizationFilter]
    public class DisbursementController : Controller
    {
        

        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private DisbursementService ds = new DisbursementService();
        private readonly ILoginService loginService = new LoginService();
        // GET: DisbursementLists
        public ActionResult Index(int? page, string status)
        {
            var disbursementList = unitOfWork.DisbursementListRepository.Get(includeProperties: "CreatedByStaff, ItemTransactions.Item, Department");
            
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            switch (status)
            {

                case "Pending":
                    disbursementList = disbursementList.Where(x => x.Status == Models.Status.Pending).ToList();
                    break;
                case "Completed":
                    disbursementList = disbursementList.Where(x => x.Status == Models.Status.Completed).ToList();
                    break;
                case "All":
                    disbursementList = disbursementList.ToList();
                    break;
                default:
                    disbursementList = disbursementList.ToList();
                    break;
            }

            if (disbursementList == null)
            {
                return HttpNotFound();
            }

            return View(disbursementList.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Print(int? id)
        {
            UnitOfWork uow = new UnitOfWork();
            
            DisbursementList disbursementList = uow.DisbursementListRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();
            //DisbursementList disbursementList = unitOfWork.DisbursementListRepository.GetByID(id);
            var pdfResult = new ActionAsPdf("Details", new { id = id });
       
           return pdfResult; 
        }

        [HttpGet]
        public ActionResult Disbursement(int? id)
        {
            DisbursementList DL = unitOfWork.DisbursementListRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();

            if (DL == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(DL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disbursement([Bind(Include = "Department, ItemTransactions")] DisbursementList model)
        {
            //UnitOfWork uow = new UnitOfWork();
            DisbursementList DL = unitOfWork.DisbursementListRepository.Get(filter: x => x.ID == model.ID, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();
            
            if (DL == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            for (int i = 0; i<DL.ItemTransactions.Count; i++)
            {
                for(int j = 0; j<model.ItemTransactions.Count; j++)
                {
                    if(DL.ItemTransactions.ToList()[i].Item.ID == model.ItemTransactions.ToList()[j].Item.ID)
                    {
                        DL.ItemTransactions.ToList()[i].TakeOverQty = model.ItemTransactions.ToList()[j].TakeOverQty;
                        
                    }
                }
            }
            DL.Completed(DL.Department.DeptRep);
            unitOfWork.DisbursementListRepository.Update(DL);
            unitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        public ActionResult Current()
        {
            var disbursementLists = unitOfWork.DisbursementListRepository.Get(filter: x => x.Status == Models.Status.Pending, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint");

            return View("CurrentDisbursements",disbursementLists.ToList());
        }

        // GET: DisbursementLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DisbursementList disbursementList = unitOfWork.DisbursementListRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department").FirstOrDefault();
            if (disbursementList == null)
            {
                return HttpNotFound();
            }
            return View(disbursementList);
        }

        // GET: DisbursementLists/Create
        public ActionResult Create()
        {
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            return View();
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
