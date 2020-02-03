using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;


namespace SSIMS.Controllers
{
    public class DeliveryOrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();

        // GET: DeliveryOrders
        public ActionResult Index()
        {
            var deliveryOrderVMs = db.DeliveryOrderVMs.Include(d => d.CreatedByStaff).Include(d => d.RepliedByStaff);
            return View(deliveryOrderVMs.ToList());
        }

        // GET: DeliveryOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryOrderVM deliveryOrderVM = db.DeliveryOrderVMs.Find(id);
            if (deliveryOrderVM == null)
            {
                return HttpNotFound();
            }
            return View(deliveryOrderVM);
        }

        // GET: DeliveryOrders/Create
        public ActionResult Create()
        {
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            return View();
        }

        // POST: DeliveryOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status,ExpectedDeliveryDate,TotalCost,PurchaseOrderID")] DeliveryOrderVM deliveryOrderVM)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryOrderVMs.Add(deliveryOrderVM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.RepliedByStaffID);
            return View(deliveryOrderVM);
        }

        // GET: DeliveryOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder PO = uow.PurchaseOrderRepository.GetByPurchaseOrderID(id);
                
            DeliveryOrderVM deliveryOrderVM = new DeliveryOrderVM(PO);
            if (deliveryOrderVM == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.RepliedByStaffID);
            return View(deliveryOrderVM);
        }

        // POST: DeliveryOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="PurchaseItems.TransactionItem")] DeliveryOrderVM deliveryOrderVM, int? id)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrder PO = uow.PurchaseOrderRepository.GetByPurchaseOrderID(id);
                //change to session clerk later 
                Staff CurrentUser = uow.StaffRepository.GetByID(10003);
                PO.Completed(CurrentUser);
                uow.PurchaseOrderRepository.Update(PO);
                uow.Save();
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.RepliedByStaffID);
            return View(deliveryOrderVM);
        }

        // GET: DeliveryOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryOrderVM deliveryOrderVM = db.DeliveryOrderVMs.Find(id);
            if (deliveryOrderVM == null)
            {
                return HttpNotFound();
            }
            return View(deliveryOrderVM);
        }

        // POST: DeliveryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryOrderVM deliveryOrderVM = db.DeliveryOrderVMs.Find(id);
            db.DeliveryOrderVMs.Remove(deliveryOrderVM);
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
