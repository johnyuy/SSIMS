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
using SSIMS.Service;


namespace SSIMS.Controllers
{
    public class DeliveryOrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();
        private PurchaseService ps = new PurchaseService();

        // GET: DeliveryOrders
        public ActionResult Index()
        {
            ps.recentPurchaseItem(uow.ItemRepository.GetByID("C001"));
            var deliveryOrders = db.DeliveryOrders.Include(d => d.CreatedByStaff).Include(d => d.RepliedByStaff);
            return View(deliveryOrders.ToList());
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
        public ActionResult Create([Bind(Include = "ID,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status,PurchaseOrderID")] DeliveryOrderVM deliveryOrderVM)
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
        public ActionResult Edit([Bind(Include = "ID, CreatedByStaffID, RepliedByStaffID, Comments, CreatedDate, ResponseDate, Status, PurchaseOrderID, TransactionItems.TransactionItem")] DeliveryOrderVM deliveryOrderVM, int? id)
        {
            if (ModelState.IsValid)
            {
                List<DocumentItem> deliveredItems = new List<DocumentItem>();
                List<TransactionItem> items = deliveryOrderVM.TransactionItems;

                foreach (TransactionItem ti in items)
                {
                    DocumentItem di = new DocumentItem(ti);
                    deliveredItems.Add(di);
                }
                //if partial delivery 
                //purchaseOrder.InProgress(uow.StaffRepository.GetByID(10002));
                //uow.PurchaseOrderRepository.Update(purchaseOrder);
                //uow.Save();

                PurchaseOrder PO = uow.PurchaseOrderRepository.GetByPurchaseOrderID(id);
                //change to session clerk later 
                Staff currentUser = uow.StaffRepository.GetByID(10003);
                PO.Completed(currentUser);
                uow.PurchaseOrderRepository.Update(PO);

                //create delivery order 
                DeliveryOrder deliveryOrder = new DeliveryOrder(currentUser, PO.Supplier, PO);
                deliveryOrder.DocumentItems = deliveredItems;
                uow.DeliveryOrderRepository.Insert(deliveryOrder);

                uow.Save();

            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.RepliedByStaffID);
            return RedirectToAction("Index");
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
