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
            DisbursementList DL = unitOfWork.DisbursementListRepository.Get(filter: x => x.ID == model.ID, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint").FirstOrDefault();
            
            for(int i = 0; i<DL.ItemTransactions.Count; i++)
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

        public ActionResult CurrentDisbursements()
        {
            var disbursementLists = unitOfWork.DisbursementListRepository.Get(filter: x => x.Status == Models.Status.Pending, includeProperties: "CreatedByStaff, ItemTransactions.Item, Department.CollectionPoint");

            return View(disbursementLists.ToList());
        }

        //public ActionResult Disbursement([Bind(Include = "deptDVM")] DisbursementViewModel model)
        //{
        //    List<RetrievalList> retrievalLists = (List<RetrievalList>)unitOfWork.RetrievalListRepository.Get(filter: x => x.Status == Models.Status.InProgress, includeProperties: "Department");
        //    var deptList = retrievalLists.Select(x => x.Department.ID).Distinct();
        //    //List<Department> deptList = (List<Department>)uow.DepartmentRepository.Get();
        //    List<DeptDisbursementViewModel> deptDVMList = new List<DeptDisbursementViewModel>();
        //    foreach (string dept in deptList)
        //    {
        //        DeptDisbursementViewModel deptDVM = ds.GenerateDeptDisbursementViewModel(dept);
        //        deptDVMList.Add(deptDVM);
        //    }
        //    return View(deptDVMList);
        //}

        //GET: RetrievalLists
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Retrieval([Bind(Include = "ROList, rivmlist")] RetrivalVM model)
        {
            
            //model.ROList[i].ID;
            //rivmlist.deptRetrievalItems.transactionItem.Item,
            //model.rivmlist[1].deptRetrievalItems[i].transactionItem.Item
            List<RequisitionOrder> ROList = model.ROList;
            foreach(RequisitionOrder RO in ROList)
            {
                RequisitionOrder NewRO = unitOfWork.RequisitionOrderRepository.GetByID(RO.ID);
                NewRO.Completed(loginService.StaffFromSession);
                unitOfWork.RequisitionOrderRepository.Update(NewRO);
                unitOfWork.Save();
            }
            

            List<RetrievalItemViewModel> rivmList = model.rivmlist;
            ds.InsertRetrievalList(rivmList);

            return RedirectToAction("Index");
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

        // POST: DisbursementLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] DisbursementList disbursementList)
        {
            if (ModelState.IsValid)
            {
                db.DisbursementLists.Add(disbursementList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.RepliedByStaffID);
            return View(disbursementList);
        }

        // GET: DisbursementLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisbursementList disbursementList = db.DisbursementLists.Find(id);
            if (disbursementList == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.RepliedByStaffID);
            return View(disbursementList);
        }

        // POST: DisbursementLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] DisbursementList disbursementList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disbursementList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", disbursementList.RepliedByStaffID);
            return View(disbursementList);
        }

        // GET: DisbursementLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisbursementList disbursementList = db.DisbursementLists.Find(id);
            if (disbursementList == null)
            {
                return HttpNotFound();
            }
            return View(disbursementList);
        }

        // POST: DisbursementLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisbursementList disbursementList = db.DisbursementLists.Find(id);
            db.DisbursementLists.Remove(disbursementList);
            db.SaveChanges();
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
