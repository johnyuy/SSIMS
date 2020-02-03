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

namespace SSIMS.Controllers
{
    public class DisbursementListsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private DisbursementService ds = new DisbursementService();

        // GET: DisbursementLists
        public ActionResult Index()
        {
            var disbursementLists = db.DisbursementLists.Include(d => d.CreatedByStaff).Include(d => d.RepliedByStaff);

            //my code for testing
            ds.GenerateDeptRetrievalList("ENGL");
            var combinedList = ds.GenerateCombinedRetrievalList();
            ds.InsertDeptRetrievalList("ENGL");
            ds.GenerateRetrievalItemViewModel(combinedList);
            if (disbursementLists == null)
            {
                return HttpNotFound();
            }


            return View(disbursementLists.ToList());
        }

        //GET: RetrievalLists
        public ActionResult Retrieval()
        {
            
            ds.GenerateDeptRetrievalList("ENGL");
            var combinedList = ds.GenerateCombinedRetrievalList();
            ds.InsertDeptRetrievalList("ENGL");
            var rivm = ds.GenerateRetrievalItemViewModel(combinedList);
            var retrievalLists = db.RetrievalLists.Include(d => d.CreatedByStaff).Include(d => d.Status);

            if(retrievalLists == null)
            {
                return HttpNotFound();
            }

            return View(rivm.ToList());
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
