using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.Service;
using SSIMS.DAL;
using SSIMS.ViewModels;
using PagedList;
using SSIMS.Filters;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class PurchaseOrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private UnitOfWork uow = new UnitOfWork();
        readonly ILoginService loginService = new LoginService();

        // GET: PurchaseOrders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.Dates = String.IsNullOrEmpty(sortOrder) ? "do_date" : "";
            ViewBag.Cost = sortOrder == "cost" ? "do_cost" : "cost";

            var purchaseOrders = uow.PurchaseOrderRepository.Get(includeProperties: "Supplier, PurchaseItems.Tender");

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
                purchaseOrders = purchaseOrders.Where(i => i.ID.ToString().Contains(searchString.ToUpper())
                                       || i.Supplier.ID.ToUpper().Contains(searchString.ToUpper()) 
                                       || i.CreatedDate.ToString().Contains(searchString.ToUpper())
                                       || i.TotalCost().ToString().Contains(searchString.ToUpper())
                                       || i.Status.ToString().ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "do_date":
                    purchaseOrders = purchaseOrders.OrderByDescending(i => i.CreatedDate);
                    break;
                case "do_cost":
                    purchaseOrders = purchaseOrders.OrderBy(i => i.TotalCost());
                    break;
                case "cost":
                    purchaseOrders = purchaseOrders.OrderByDescending(i => i.TotalCost());
                    break;
                default:
                    purchaseOrders = purchaseOrders.OrderBy(i => i.CreatedDate);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<PurchaseOrderVM> vm = new List<PurchaseOrderVM>();

            foreach (PurchaseOrder PO in purchaseOrders)
            {
                vm.Add(new PurchaseOrderVM(PO));
            }

            return View(vm.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "Supplier, CreatedByStaff, RepliedByStaff, PurchaseItems.Tender.Item ").FirstOrDefault();

            Staff staff = loginService.StaffFromSession;
            //ViewBag.staffrole = staff.StaffRole;

            // to change later // changed already
            purchaseOrder.Approve(staff);
            uow.PurchaseOrderRepository.Update(purchaseOrder);
            uow.Save();
            Debug.WriteLine("Purchase Order Approved");
            PurchaseOrderVM vm = new PurchaseOrderVM(purchaseOrder);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "Supplier, CreatedByStaff, RepliedByStaff, PurchaseItems.Tender.Item ").FirstOrDefault();

            Staff staff = loginService.StaffFromSession;
            //ViewBag.staffrole = staff.StaffRole;

            // to change later //changed already
            purchaseOrder.Rejected(staff);
            uow.PurchaseOrderRepository.Update(purchaseOrder);
            uow.Save();
            Debug.WriteLine("Purchase Order Rejected");
            PurchaseOrderVM vm = new PurchaseOrderVM(purchaseOrder);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "Supplier, CreatedByStaff, RepliedByStaff, PurchaseItems.Tender.Item ").FirstOrDefault();

            // to change later 
            purchaseOrder.Cancelled(uow.StaffRepository.GetByID(10002));
            uow.PurchaseOrderRepository.Update(purchaseOrder);
            uow.Save();
            Debug.WriteLine("Purchase Order Cancelled");
            PurchaseOrderVM vm = new PurchaseOrderVM(purchaseOrder);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult DeliveryOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "Supplier, CreatedByStaff, RepliedByStaff, PurchaseItems.Tender.Item ").FirstOrDefault();

            // to change later 
            Debug.WriteLine("Delivery Order ");
            PurchaseOrderVM vm = new PurchaseOrderVM(purchaseOrder);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", "DeliveryOrders", new { id = id });
        }



        public ActionResult ViewDeliveryOrder(int? id)
        {
            return RedirectToAction("Details", "DeliveryOrders", new { id = id });
        }



        // GET: PurchaseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Staff staff = loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;

            PurchaseOrder purchaseOrder = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "Supplier, CreatedByStaff, RepliedByStaff, PurchaseItems.Tender.Item ").FirstOrDefault();
            PurchaseOrderVM vm = new PurchaseOrderVM(purchaseOrder);

            if (vm.Status.ToString() == "InProgress" || vm.Status.ToString() == "Completed")
            {
                List<DeliveryOrder> deliveryOrders = uow.DeliveryOrderRepository.Get(x => x.PurchaseOrder.ID == vm.ID).ToList();
                vm.DeliveryOrders = deliveryOrders;
            }

            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(vm);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID");
            return View();
        }

        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ExpectedDeliveryDate,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.RepliedByStaffID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.RepliedByStaffID);
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ExpectedDeliveryDate,CreatedByStaffID,RepliedByStaffID,Comments,CreatedDate,ResponseDate,Status")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.CreatedByStaffID);
            ViewBag.RepliedByStaffID = new SelectList(db.Staffs, "ID", "UserAccountID", purchaseOrder.RepliedByStaffID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
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
