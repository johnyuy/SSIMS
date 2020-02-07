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
        public ActionResult Index()
        {
            var disbursementLists = db.DisbursementLists.Include(d => d.CreatedByStaff).Include(d => d.RepliedByStaff);

            //my code for testing
            
            if (disbursementLists == null)
            {
                return HttpNotFound();
            }


            return View(disbursementLists.ToList());
        }

        //GET: RetrievalLists
        public ActionResult Retrieval()
        {

            List<TransactionItem> combinedRL = ds.ViewCombinedRetrievalList();
            var rivm = ds.ViewRetrievalItemViewModel(combinedRL);
            var retrievalLists = db.RetrievalLists.Include(d => d.CreatedByStaff).Include(d => d.Status);

            if(retrievalLists == null)
            {
                return HttpNotFound();
            }

            return View(rivm);
        }

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
            DisbursementList disbursementList = db.DisbursementLists.Find(id);
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
