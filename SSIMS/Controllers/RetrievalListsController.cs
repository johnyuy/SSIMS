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

namespace SSIMS.Controllers
{
    public class RetrievalListsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: RetrievalLists
        public ActionResult Index(int? page, string status)
        {
            var retrievalList = uow.RetrievalListRepository.Get(includeProperties: "CreatedByStaff, ItemTransactions, Department").ToList();
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(retrievalList.ToPagedList(pageNumber, pageSize));
        }


        // GET: RetrievalLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetrievalList retrievalList = db.RetrievalLists.Find(id);
            if (retrievalList == null)
            {
                return HttpNotFound();
            }
            return View(retrievalList);
        }

        // GET: RetrievalLists/Create
        public ActionResult Create()
        {
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            return View();
        }

        // POST: RetrievalLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] RetrievalList retrievalList)
        {
            if (ModelState.IsValid)
            {
                db.RetrievalLists.Add(retrievalList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.RepliedByStaffID);
            return View(retrievalList);
        }

        // GET: RetrievalLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetrievalList retrievalList = db.RetrievalLists.Find(id);
            if (retrievalList == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.RepliedByStaffID);
            return View(retrievalList);
        }

        // POST: RetrievalLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] RetrievalList retrievalList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(retrievalList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", retrievalList.RepliedByStaffID);
            return View(retrievalList);
        }

        // GET: RetrievalLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RetrievalList retrievalList = db.RetrievalLists.Find(id);
            if (retrievalList == null)
            {
                return HttpNotFound();
            }
            return View(retrievalList);
        }

        // POST: RetrievalLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RetrievalList retrievalList = db.RetrievalLists.Find(id);
            db.RetrievalLists.Remove(retrievalList);
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
