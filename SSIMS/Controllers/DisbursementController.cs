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
using System.Diagnostics;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class DisbursementController : Controller
    {


        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private DisbursementService ds = new DisbursementService();
        private readonly ILoginService loginService = new LoginService();
        // GET: DisbursementLists
        public ActionResult Index(int? page, string status="Pending")
        {
            UnitOfWork uow = new UnitOfWork();
            

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            var disbursementList = new List<DisbursementList>();
            Staff staff = loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;
            if (staff.StaffRole == "DeptHead" || staff.StaffRole == "DeptRep")
            {
                disbursementList = uow.DisbursementListRepository.Get(filter: x => x.Department.ID == staff.DepartmentID).ToList();
                switch (status)
                {

                    case "Pending":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Pending).ToList();
                        break;
                    case "Completed":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Completed).ToList();
                        break;
                    case "Rejected":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Rejected).ToList();
                        break;

                    case "All":
                        disbursementList = disbursementList.ToList();
                        break;
                }

                if (disbursementList == null)
                {
                    return HttpNotFound();
                }
                //ViewBag.DisbursementStatus = status;
            }

            if (staff.StaffRole == "Manager" || staff.StaffRole == "Supervisor" || staff.StaffRole == "Clerk")
            {
                disbursementList = uow.DisbursementListRepository.Get(includeProperties: "CreatedByStaff, ItemTransactions.Item, Department").ToList();
                switch (status)
                {

                    case "Pending":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Pending).ToList();
                        break;
                    case "Completed":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Completed).ToList();
                        break;
                    case "Rejected":
                        disbursementList = disbursementList.Where(x => x.Status == Models.Status.Rejected).ToList();
                        break;

                    case "All":
                        disbursementList = disbursementList.ToList();
                        break;
                }

                if (disbursementList == null)
                {
                    return HttpNotFound();
                }
                ViewBag.DisbursementStatus = status;
            }

            return View(disbursementList.ToPagedList(pageNumber, pageSize));

        }

        //[AllowAnonymous]
        public ActionResult Print(int? id)
        {
            UnitOfWork uow = new UnitOfWork();

            DisbursementList disbursementList = uow.DisbursementListRepository.Get(filter: x => x.ID == id.Value, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();


            var fff = new PartialViewAsPdf
            {
                ViewName = "Details",
                Model = disbursementList
            };

            return fff;
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
        public ActionResult CompleteDisbursement(string id="", string reply = "")
        {
            if(!int.TryParse(id, out int Id))
                return RedirectToAction("Disbursement", new { id });
            UnitOfWork uow = new UnitOfWork();
            DisbursementList DL = uow.DisbursementListRepository.Get(filter: x => x.ID == Id, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();
            Debug.WriteLine(reply);
            List<int> takeover = new List<int>();
            List<string> reason = new List<string>();

            string[] sets = reply.Split('&');

            for(int i = 0; i < sets.Length; i++)
            {
                string value = sets[i].Split('=')[1];
                if (i % 2 == 0)
                {

                    takeover.Add(int.Parse(value));
                    Debug.WriteLine(int.Parse(value));
                }
                else
                {
                    reason.Add(value);
                    Debug.WriteLine(value);
                }

            }

            //update takeover qty
            for (int i = 0; i < DL.ItemTransactions.Count; i++)
            {
                DL.ItemTransactions.ToList()[i].TakeOverQty = takeover[i];
                DL.ItemTransactions.ToList()[i].Reason = reason[i];
            }

            //update
            Staff depRep = uow.StaffRepository.Get(filter: x=>x.Department.ID == DL.Department.ID && x.StaffRole == "DeptRep").FirstOrDefault();
            DL.Completed(depRep);
            
            uow.DisbursementListRepository.Update(DL);
            uow.Save();


            return RedirectToAction("Current");
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


        [HttpGet]
        public JsonResult Verify(string p="", string d="")
        {
            string s = "false";
            UnitOfWork uow = new UnitOfWork();
            Debug.WriteLine("Received : " + p + ", checking with DL" + d );
            int.TryParse(d, out int i);

            if (ds.VerifyOTP(i, p, uow))
                s = "true";

            return Json(s, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfWork uow = new UnitOfWork();
            DisbursementList disbursementList = uow.DisbursementListRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department").FirstOrDefault();
            if (disbursementList == null)
            {
                return HttpNotFound();
            }
            InventoryService inventoryService = new InventoryService();
            Staff clerk = loginService.StaffFromSession;
            inventoryService.ProcessRejectedDisbursement(clerk, disbursementList.ID);
            disbursementList.Rejected(clerk);
            uow.DisbursementListRepository.Update(disbursementList);
            uow.Save();
            return RedirectToAction("Index");
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
