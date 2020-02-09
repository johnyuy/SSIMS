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
using System.Diagnostics;
using SSIMS.Service;
using PagedList;


namespace SSIMS.Controllers
{
    public class DeliveryOrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();
        private ILoginService loginService = new LoginService();
        private PurchaseService ps = new PurchaseService();

        // GET: DeliveryOrders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.Dates = String.IsNullOrEmpty(sortOrder) ? "do_date" : "";

            var deliveryOrders = uow.DeliveryOrderRepository.Get(includeProperties: "CreatedByStaff, DocumentItems, PurchaseOrder");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                deliveryOrders = deliveryOrders.Where(i => i.ID.ToString().Contains(searchString.ToUpper())
                                       || i.CreatedByStaff.Name.ToUpper().Contains(searchString.ToUpper())
                                       || i.CreatedDate.ToString().Contains(searchString.ToUpper())
                                       || i.PurchaseOrder.ID.ToString().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "do_date":
                    deliveryOrders = deliveryOrders.OrderByDescending(i => i.CreatedDate);
                    break;
                default:
                    deliveryOrders = deliveryOrders.OrderBy(i => i.CreatedDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<DeliveryOrderVM> vms = new List<DeliveryOrderVM>();

            foreach (DeliveryOrder DO in deliveryOrders)
            {
                vms.Add(new DeliveryOrderVM(DO));
            }

            return View(vms.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ViewPurchaseOrder(int id)
        {
            return RedirectToAction("Details", "PurchaseOrders", new { id = id });
        }

        // GET: DeliveryOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DeliveryOrder deliveryOrder = uow.DeliveryOrderRepository.Get(filter: x => x.ID == id, includeProperties: "CreatedByStaff, DocumentItems.Item, PurchaseOrder").FirstOrDefault();
            DeliveryOrderVM vm = new DeliveryOrderVM(deliveryOrder);

            if (vm == null)
            {
                return HttpNotFound();
            }
            return View(vm);
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

            //If PO in progress create a form for the remainder items 
            if(PO.Status.ToString() == "InProgress")
            {
                foreach (DeliveryOrder DO in PO.DeliveryOrders)
                {
                    foreach(DocumentItem di in DO.DocumentItems)
                    {
                        foreach(PurchaseItem pi in PO.PurchaseItems)
                        {
                            if(pi.Tender.Item.ID == di.Item.ID)
                            {
                                pi.Qty = pi.Qty - di.Qty;
                            }
                        }
                    }
                }
            }
                
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
        public ActionResult Edit([Bind(Include = "ID, CreatedByStaffID, RepliedByStaffID, Comments, Supplier, CreatedDate, ResponseDate, Status, PurchaseOrderID, TransactionItems")] DeliveryOrderVM deliveryOrderVM, int? id)
        {
            if (ModelState.IsValid)
            {

                List<DocumentItem> deliveredItems = new List<DocumentItem>();
                List<TransactionItem> items = deliveryOrderVM.TransactionItems;

                //Check if Delivery Incomplete
                bool incomplete = false;
                foreach (TransactionItem ti in items)
                {
                    if (ti.TakeOverQty < ti.HandOverQty)
                    {
                        incomplete = true;
                    }
                    DocumentItem di = new DocumentItem(ti, uow);
                    if(di.Qty != 0)
                    {
                        deliveredItems.Add(di);
                    }
                }
                
                //Update PurchaseOrder Status
                PurchaseOrder PO = uow.PurchaseOrderRepository.GetByPurchaseOrderID(id);
                if (incomplete)
                {
                    Debug.WriteLine("Incomplete True");
                    PO.InProgress();
                    uow.PurchaseOrderRepository.Update(PO);
                    uow.Save();
                }
                else
                {
                    Debug.WriteLine("Complete True");
                    PO.Completed();
                    uow.PurchaseOrderRepository.Update(PO);
                    uow.Save();
                }

                Staff currentUser = loginService.StaffFromSession;

                //Create DeliveryOrder

                DeliveryOrder deliveryOrder = new DeliveryOrder(currentUser, PO.Supplier, PO);
                deliveryOrder.DocumentItems = deliveredItems;
                bool result = uow.StockCardEntryRepository.ProcessDeliveryOrderAcceptance(deliveryOrder);

                if (result){
                    uow.DeliveryOrderRepository.Insert(deliveryOrder);
                    uow.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.CreatedByStaffID);
                    ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", deliveryOrderVM.RepliedByStaffID);
                    return View(deliveryOrderVM);
                }
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
